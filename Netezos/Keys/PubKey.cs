using System;
using System.Text;
using Org.BouncyCastle.Utilities.Encoders;

using Netezos.Encoding;
using Netezos.Keys.Crypto;
using Netezos.Utils;
using Hex = Netezos.Encoding.Hex;

namespace Netezos.Keys
{
    public class PubKey
    {
        public string Address
        {
            get
            {
                if (_Address == null)
                {
                    using (Store.Unlock())
                    {
                        _Address = Base58.Convert(Blake2b.GetDigest(Store.Secret, 160), Curve.AddressPrefix);
                    }
                }
                
                return _Address;
            }
        }
        string _Address;

        internal readonly ICurve Curve;
        internal readonly ISecretStore Store;

        internal PubKey(byte[] bytes, ECKind kind, bool flush = false)
        {
            if (bytes.Length < 32)
                throw new ArgumentException("Invalid public key length", nameof(bytes));

            Curve = Curves.GetCurve(kind);
            Store = new PlainSecretStore(bytes);

            if (flush) bytes.Flush();
        }

        public byte[] GetBytes()
        {
            using (Store.Unlock())
            {
                var bytes = new byte[Store.Secret.Length];
                Buffer.BlockCopy(Store.Secret, 0, bytes, 0, Store.Secret.Length);
                return bytes;
            }
        }

        public string GetBase58()
        {
            using (Store.Unlock())
            {
                return Base58.Convert(Store.Secret, Curve.PublicKeyPrefix);
            }
        }

        public bool Verify(byte[] data, byte[] signature)
        {
            using (Store.Unlock())
            {
                return Curve.Verify(data, signature, Store.Secret);
            }
        }

        public bool Verify(string message, string signature)
        {
            using (Store.Unlock())
            {
                var messageBytes = System.Text.Encoding.UTF8.GetBytes(message);
                var signatureBytes = Base58.Parse(signature, Curve.SignaturePrefix);

                return Curve.Verify(messageBytes, signatureBytes, Store.Secret);
            }
        }

        public override string ToString() => GetBase58();

        #region static
        public static PubKey FromBytes(byte[] bytes, ECKind kind = ECKind.Ed25519)
            => new PubKey(bytes, kind);

        public static PubKey FromHex(string hex, ECKind kind = ECKind.Ed25519)
            => new PubKey(Hex.Parse(hex), kind, true);

        public static PubKey FromBase64(string base64, ECKind kind = ECKind.Ed25519)
            => new PubKey(Base64.Decode(base64), kind, true);

        public static PubKey FromBase58(string base58)
        {
            var curve = Curves.GetCurve(base58.Substring(0, 4));
            var bytes = Base58.Parse(base58, curve.PublicKeyPrefix);

            return new PubKey(bytes, curve.Kind, true);
        }
        #endregion
    }
}
