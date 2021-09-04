using System;
using System.ComponentModel;
using System.Linq;
using Netezos.Encoding;
using Netezos.Utils;

namespace Netezos.Keys
{
    public class HDKey
    {
        public Key Key
        {
            get
            {
                if (_Key == null)
                {
                    using (Store.Unlock())
                    {
                        var privateKey = Store.Data.GetBytes(0, 32);
                        _Key = new Key(privateKey, Curve.Kind, true, _PubKey);
                    }
                }

                return _Key;
            }
        }
        Key _Key;
        
        public PubKey PubKey
        {
            get
            {
                if (_PubKey == null)
                {
                    using (Store.Unlock())
                    {
                        _PubKey = new PubKey(Curve.GetPublicKey(Store.Data), Curve.Kind, true);
                    }
                }

                return _PubKey;
            }
        }

        PubKey _PubKey;
        
        public HDPubKey HdPubKey
        {
            get
            {
                if (_HDPubKey == null)
                {
                    if (_PubKey == null)
                    {
                        using (Store.Unlock())
                        {
                            _HDPubKey = new HDPubKey(Curve.GetPublicKey(Store.Data), ChainCode, Hd.Kind, Curve.Kind, true);
                        }
                    }
                    else
                    {
                        _HDPubKey = HDPubKey.FromPubKey(_PubKey, ChainCode, Hd.Kind);
                    }
                }
                return _HDPubKey;
            }
        }

        HDPubKey _HDPubKey;

        readonly Curve Curve;
        readonly HDStandard Hd;
        readonly ISecretStore Store;
        
        //TODO Consider moving to ISecretStore
        public readonly byte[] ChainCode;

        public HDKey() : this(HDStandardKind.Slip10, ECKind.Ed25519) { }

        public HDKey(HDStandardKind hdStandard) : this(hdStandard, ECKind.Ed25519) { }

        public HDKey(HDStandardKind hdStandard, ECKind ecKind)
        {
            Curve = Curve.FromKind(ecKind);
            Hd = HDStandard.FromKind(hdStandard);
            var bytes = RNG.GetBytes(64);
            Store = new PlainSecretStore(bytes.GetBytes(0,32));
            ChainCode = bytes.GetBytes(32, 32);
            bytes.Flush();
        }

        public HDKey(byte[] privateKey, byte[] chainCode, HDStandardKind hdStandard, ECKind ecKind, bool flush = false)
        {
            if (privateKey?.Length != 32)
                throw new ArgumentException("Invalid private key length", nameof(privateKey));
            if (chainCode?.Length != 32)
                throw new ArgumentException("Invalid chainCode  length", nameof(chainCode));

            Curve = Curve.FromKind(ecKind);
            Hd = HDStandard.FromKind(hdStandard);
            Store = new PlainSecretStore(privateKey);
            ChainCode = chainCode;
            if (flush) privateKey.Flush();
        }

        public HDKey Derive(uint index, bool hardened = false)
        {
            using (Store.Unlock())
            {
                var (privateKey, chainCode) = Hd.GetChildPrivateKey(Curve, Store.Data, ChainCode, index);
                return new HDKey(privateKey, chainCode, Hd.Kind, Curve.Kind);
            }
        }

        public HDKey Derive(HDPath path) 
        {
            using (Store.Unlock())
            {
                var (privateKey, chainCode) = path.Indexes
                    .Aggregate((Store.Data, ChainCode), (mks, next) => Hd.GetChildPrivateKey(Curve, mks.Data, mks.ChainCode, next));

                return new HDKey(privateKey, chainCode, Hd.Kind, Curve.Kind);
            }
        }

        public byte[] GetBytes()
        {
            using (Store.Unlock())
            {
                var bytes = new byte[Store.Data.Length];
                Buffer.BlockCopy(Store.Data, 0, bytes, 0, Store.Data.Length);
                return bytes;
            }
        }

        #region static

        public static HDKey FromBytes(byte[] bytes, HDStandardKind hdStandard = HDStandardKind.Slip10,
            ECKind ecKind = ECKind.Ed25519)
        {
            var (privateKey, chainCode) = HDStandard.FromKind(hdStandard).GenerateMasterKey(Curve.FromKind(ecKind), bytes);
            return new HDKey(privateKey, chainCode, hdStandard, ecKind, true);
        }

        public static HDKey FromHex(string hex, HDStandardKind hdStandard = HDStandardKind.Slip10,
            ECKind ecKind = ECKind.Ed25519)
        {
            var (privateKey, chainCode) = HDStandard.FromKind(hdStandard).GenerateMasterKey(Curve.FromKind(ecKind), Hex.Parse(hex));
            return new HDKey(privateKey, chainCode, hdStandard, ecKind, true);
        }

        public static HDKey FromBase64(string privateKey, string chainCode, HDStandardKind hdStandard = HDStandardKind.Slip10, ECKind ecKind = ECKind.Ed25519)
            => new HDKey(Base64.Parse(privateKey), Base64.Parse(chainCode), hdStandard, ecKind, true);

        public static HDKey FromSeed(byte[] seed, HDStandardKind hdStandard = HDStandardKind.Slip10, ECKind ecKind = ECKind.Ed25519)
        {
            var (privateKey, chainCode) = HDStandard.FromKind(hdStandard).GenerateMasterKey(Curve.FromKind(ecKind), seed);
            return new HDKey(privateKey, chainCode, hdStandard, ecKind, true);
        }

        public static HDKey FromMnemonic(Mnemonic mnemonic, HDStandardKind hdStandard = HDStandardKind.Slip10, ECKind ecKind = ECKind.Ed25519)
        {
            var seed = mnemonic.GetSeed();
            var key = FromSeed(seed, hdStandard, ecKind);
            seed.Flush();
            return key;
        }

        public static HDKey FromMnemonic(Mnemonic mnemonic, string passphrase, HDStandardKind hdStandard = HDStandardKind.Slip10, ECKind ecKind = ECKind.Ed25519)
        {
            var seed = mnemonic.GetSeed(passphrase);
            var key = FromSeed(seed, hdStandard, ecKind);
            seed.Flush();
            return key;
        }
        #endregion
    }
}
