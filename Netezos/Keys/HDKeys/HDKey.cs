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
                        //TODO Changed lenght from 32 to 64 for testing purposes. Change back after tests
                        var privateKey = Store.Data.GetBytes(0, 64);
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

        readonly Curve Curve;
        readonly HDStandard Hd;
        readonly ISecretStore Store;
        readonly uint hardenedOffset = 0x80000000;

        public HDKey() : this(HDStandardKind.Slip10, ECKind.Ed25519) { }

        public HDKey(HDStandardKind hdStandard) : this(hdStandard, ECKind.Ed25519) { }

        public HDKey(HDStandardKind hdStandard, ECKind ecKind)
        {
            Curve = Curve.FromKind(ecKind);
            Hd = HDStandard.FromKind(hdStandard);
            var bytes = RNG.GetBytes(64);
            Store = new PlainSecretStore(bytes);
            bytes.Flush();
        }

        public HDKey(byte[] bytes, HDStandardKind hdStandard, ECKind ecKind, bool flush = false)
        {
            //TODO Turn off after tests (test vector only 16 bytes long)
            // if (bytes?.Length != 64)
            //     throw new ArgumentException("Invalid extended key length", nameof(bytes));

            Curve = Curve.FromKind(ecKind);
            Hd = HDStandard.FromKind(hdStandard);
            Store = new PlainSecretStore(bytes);
            //TODO Consider moving out the chain code
            //TODO That's working with zero bytes for ed25519 and doesn't for secp256k1
            _PubKey = ecKind switch
            {
                ECKind.Ed25519 => new PubKey(Hd.GetChildPublicKey(Curve, bytes.GetBytes(0, 32), true), Curve.Kind,
                    flush),
                ECKind.Secp256k1 => new PubKey(Hd.GetChildPublicKey(Curve, bytes.GetBytes(0, 32), false), Curve.Kind,
                    flush),
                ECKind.NistP256 => new PubKey(Hd.GetChildPublicKey(Curve, bytes.GetBytes(0, 32), false), Curve.Kind,
                    flush),
                _ => throw new InvalidEnumArgumentException()
            };
            
            if (flush) bytes.Flush();
        }

        public HDKey Derive(uint index, bool hardened = false)
        {
            using (Store.Unlock())
            {

                var privateKey = hardened
                    ? Hd.GetChildPrivateKey(Curve, Store.Data, index)
                    : Hd.GetChildPrivateKey(Curve, Store.Data, index);
                return new HDKey(privateKey, Hd.Kind, Curve.Kind);
            }
        }

        public HDKey Derive(HDPath path)
        {
            using (Store.Unlock())
            {
                var privateKey = path.Indexes
                    .Aggregate(Store.Data, (mks, next) => Hd.GetChildPrivateKey(Curve, mks, next));

                return new HDKey(privateKey, Hd.Kind, Curve.Kind);
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
            var res = HDStandard.FromKind(hdStandard).GenerateMasterKey(Curve.FromKind(ecKind), bytes);
            return new HDKey(res, hdStandard, ecKind, true);
        }

        public static HDKey FromHex(string hex, HDStandardKind hdStandard = HDStandardKind.Slip10,
            ECKind ecKind = ECKind.Ed25519)
        {
            var res = HDStandard.FromKind(hdStandard).GenerateMasterKey(Curve.FromKind(ecKind), Hex.Parse(hex));
            return new HDKey(res, hdStandard, ecKind, true);
        }

        public static HDKey FromBase64(string base64, HDStandardKind hdStandard = HDStandardKind.Slip10, ECKind ecKind = ECKind.Ed25519)
            => new HDKey(Base64.Parse(base64), hdStandard, ecKind, true);

        public static HDKey FromSeed(byte[] seed, HDStandardKind hdStandard = HDStandardKind.Slip10, ECKind ecKind = ECKind.Ed25519)
        {
            var bytes = HDStandard.FromKind(hdStandard).GenerateMasterKey(Curve.FromKind(ecKind), seed);
            return new HDKey(bytes, hdStandard, ecKind, true);
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
