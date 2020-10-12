using System;
using System.Text;
using Org.BouncyCastle.Utilities.Encoders;

using Netezos.Keys.Crypto;
using Netezos.Utils;
using Hex = Netezos.Utils.Hex;

namespace Netezos.Keys
{
    public class Key
    {
        public PubKey PubKey
        {
            get
            {
                if (_PubKey == null)
                {
                    using (Store.Unlock())
                    {
                        _PubKey = new PubKey(Curve.GetPublicKey(Store.Secret), Curve.Kind, true);
                    }
                }

                return _PubKey;
            }
        }
        PubKey _PubKey;

        readonly ICurve Curve;
        readonly ISecretStore Store;

        public Key() : this(RNG.GetNonZeroBytes(32), ECKind.Ed25519, true) { } //TODO: check key strength

        public Key(ECKind kind) : this(RNG.GetNonZeroBytes(32), kind, true) { } //TODO: check key strength

        internal Key(byte[] bytes, ECKind kind, bool flush = false)
        {
            if (bytes.Length < 32)
                throw new ArgumentException("Invalid private key length", nameof(bytes));

            Curve = Curves.GetCurve(kind);
            
            var privateKey = Curve.GetPrivateKey(bytes);
            Store = new PlainSecretStore(privateKey);

            privateKey.Flush();
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
                return Base58.Convert(Store.Secret, Curve.PrivateKeyPrefix);
            }
        }

        public Signature Sign(byte[] bytes)
        {
            using (Store.Unlock())
            {
                return Curve.Sign(bytes, Store.Secret);
            }
        }

        public Signature Sign(string message)
        {
            using (Store.Unlock())
            {
                return Curve.Sign(Encoding.UTF8.GetBytes(message), Store.Secret);
            }
        }

        public bool Verify(byte[] data, byte[] signature) => PubKey.Verify(data, signature);

        public bool Verify(string message, string signature) => PubKey.Verify(message, signature);

        public override string ToString() => GetBase58();

        #region static
        public static Key FromBytes(byte[] bytes, ECKind kind = ECKind.Ed25519)
            => new Key(bytes, kind);

        public static Key FromHex(string hex, ECKind kind = ECKind.Ed25519)
            => new Key(Hex.Parse(hex), kind, true);

        public static Key FromBase64(string base64, ECKind kind = ECKind.Ed25519)
            => new Key(Base64.Decode(base64), kind, true);

        public static Key FromBase58(string base58)
        {
            var curve = Curves.GetCurve(base58.Substring(0, 4));
            var bytes = Base58.Parse(base58, curve.PrivateKeyPrefix);

            return new Key(bytes, curve.Kind, true);
        }

        public static Key FromMnemonic(Mnemonic mnemonic)
            => new Key(mnemonic.GetSeed(), ECKind.Ed25519, true);

        public static Key FromMnemonic(Mnemonic mnemonic, string email, string password)
            => new Key(mnemonic.GetSeed($"{email}{password}"), ECKind.Ed25519, true);

        public static Key FromMnemonic(Mnemonic mnemonic, string passphrase, ECKind kind = ECKind.Ed25519)
            => new Key(mnemonic.GetSeed(passphrase), kind, true);
        #endregion
    }
}
