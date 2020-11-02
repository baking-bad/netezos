using System;
using Netezos.Encoding;
using Netezos.Utils;

namespace Netezos.Keys
{
    public class PubKey
    {
        string _Address;
        public string Address
        {
            get
            {
                if (_Address == null)
                {
                    using (Store.Unlock())
                    {
                        _Address = Base58.Convert(Blake2b.GetDigest(Store.Data, 160), Curve.AddressPrefix);
                    }
                }
                
                return _Address;
            }
        }

        internal readonly Curve Curve;
        internal readonly ISecretStore Store;

        internal PubKey(byte[] bytes, ECKind kind, bool flush = false)
        {
            if (kind == ECKind.Ed25519 && bytes.Length != 32 || kind != ECKind.Ed25519 && bytes.Length != 33)
                throw new ArgumentException("Invalid public key length", nameof(bytes));

            Curve = Curve.FromKind(kind);
            Store = new PlainSecretStore(bytes);
            if (flush) bytes.Flush();
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

        public string GetBase58()
        {
            using (Store.Unlock())
            {
                return Base58.Convert(Store.Data, Curve.PublicKeyPrefix);
            }
        }

        public bool Verify(byte[] data, byte[] signature)
        {
            using (Store.Unlock())
            {
                return Curve.Verify(data, signature, Store.Data);
            }
        }

        public bool Verify(string message, string signature)
        {
            using (Store.Unlock())
            {
                var messageBytes = Utf8.Parse(message);
                var signatureBytes = Base58.Parse(signature, Curve.SignaturePrefix);

                return Curve.Verify(messageBytes, signatureBytes, Store.Data);
            }
        }

        public override string ToString() => GetBase58();

        #region static
        public static PubKey FromBytes(byte[] bytes, ECKind kind = ECKind.Ed25519)
            => new PubKey(bytes, kind);

        public static PubKey FromHex(string hex, ECKind kind = ECKind.Ed25519)
            => new PubKey(Hex.Parse(hex), kind, true);

        public static PubKey FromBase64(string base64, ECKind kind = ECKind.Ed25519)
            => new PubKey(Base64.Parse(base64), kind, true);

        public static PubKey FromBase58(string base58)
        {
            var curve = Curve.FromPrefix(base58.Substring(0, 4));
            var bytes = Base58.Parse(base58, curve.PublicKeyPrefix);

            return new PubKey(bytes, curve.Kind, true);
        }
        #endregion
    }
}
