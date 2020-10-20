using System;
using System.Collections.Generic;
using System.Text;
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
                        _Key = new Key(privateKey, Curve.Kind, true);
                    }
                }

                return _Key;
            }
        }
        Key _Key;

        readonly Curve Curve;
        readonly HDStandard Hd;
        readonly ISecretStore Store;

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

        internal HDKey(byte[] bytes, HDStandardKind hdStandard, ECKind ecKind, bool flush = false)
        {
            if (bytes?.Length != 64)
                throw new ArgumentException("Invalid extended key length", nameof(bytes));

            Curve = Curve.FromKind(ecKind);
            Hd = HDStandard.FromKind(hdStandard);
            Store = new PlainSecretStore(bytes);
            if (flush) bytes.Flush();
        }

        public HDKey Derive(uint index, bool hardened = false)
        {
            using (Store.Unlock())
            {
                //TODO: Hd.GetChildPrivateKey(Curve, Store.Data, ...
                throw new NotImplementedException();
            }
        }

        public HDKey Derive(HDPath path)
        {
            using (Store.Unlock())
            {
                //TODO: Hd.GetChildPrivateKey(Curve, Store.Data, ...
                throw new NotImplementedException();
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
        public static HDKey FromBytes(byte[] bytes, HDStandardKind hdStandard = HDStandardKind.Slip10, ECKind ecKind = ECKind.Ed25519)
            => new HDKey(bytes, hdStandard, ecKind);

        public static HDKey FromHex(string hex, HDStandardKind hdStandard = HDStandardKind.Slip10, ECKind ecKind = ECKind.Ed25519)
            => new HDKey(Hex.Parse(hex), hdStandard, ecKind, true);

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
