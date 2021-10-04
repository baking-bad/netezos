using System;
using System.Linq;
using Netezos.Encoding;
using Netezos.Utils;

namespace Netezos.Keys
{
    public class HDKey
    {
        public PubKey PubKey => _PubKey ??= new PubKey(Curve.GetPublicKey(Key.GetBytes()), Curve.Kind, true);
        PubKey _PubKey;
        
        public HDPubKey HdPubKey
        {
            get
            {
                return _HDPubKey ??= _PubKey == null
                    ? new HDPubKey(Curve.GetPublicKey(Key.GetBytes()), ChainCode, Hd.Kind, Curve.Kind, true)
                    : HDPubKey.FromPubKey(_PubKey, ChainCode, Hd.Kind);
            }
        }
        HDPubKey _HDPubKey;

        readonly Curve Curve;
        readonly HDStandard Hd;
        
        public readonly byte[] ChainCode;
        public readonly Key Key;

        public HDKey(HDStandardKind hdStandard = HDStandardKind.Slip10, ECKind ecKind = ECKind.Ed25519)
        {
            Curve = Curve.FromKind(ecKind);
            Hd = HDStandard.FromKind(hdStandard);
            var bytes = RNG.GetBytes(64);
            Key = Key.FromBytes(bytes.GetBytes(0, 32), Curve.Kind);
            ChainCode = bytes.GetBytes(32, 32);
            bytes.Flush();
        }

        HDKey(byte[] privateKey, byte[] chainCode, HDStandardKind hdStandard, ECKind ecKind, bool flush = false)
        {
            if (privateKey?.Length != 32)
                throw new ArgumentException("Invalid private key length", nameof(privateKey));
            if (chainCode?.Length != 32)
                throw new ArgumentException("Invalid chainCode  length", nameof(chainCode));

            Curve = Curve.FromKind(ecKind);
            Hd = HDStandard.FromKind(hdStandard);
            Key = Key.FromBytes(privateKey, Curve.Kind);
            ChainCode = chainCode;
            if (flush) privateKey.Flush();
        }

        public HDKey Derive(uint index, bool hardened = false)
        {
            var (privateKey, chainCode) = Hd.GetChildPrivateKey(Curve, Key.GetBytes(), ChainCode, index);
            return new HDKey(privateKey, chainCode, Hd.Kind, Curve.Kind);
        }

        public HDKey Derive(HDPath path) 
        {
            var (privateKey, chainCode) = path.Indexes
                .Aggregate((PrivKey: Key.GetBytes(), ChainCode), (mks, next) => Hd.GetChildPrivateKey(Curve, mks.PrivKey, mks.ChainCode, next));

            return new HDKey(privateKey, chainCode, Hd.Kind, Curve.Kind);
        }

        public HDKey Derive(string path)
        {
            return Derive(HDPath.Parse(path));
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
        
        public static HDKey FromMnemonic(Mnemonic mnemonic, string passphrase = "", HDStandardKind hdStandard = HDStandardKind.Slip10, ECKind ecKind = ECKind.Ed25519)
        {
            var seed = mnemonic.GetSeed(passphrase);
            var key = FromSeed(seed, hdStandard, ecKind);
            seed.Flush();
            return key;
        }
        #endregion
    }
}
