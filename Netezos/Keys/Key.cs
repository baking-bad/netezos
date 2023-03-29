using Netezos.Encoding;

namespace Netezos.Keys
{
    public class Key
    {
        public string Address => PubKey.Address;
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
        PubKey? _PubKey;

        internal readonly Curve Curve;
        internal readonly ISecretStore Store;

        public Key(ECKind kind = ECKind.Ed25519)
        {
            Curve = Curve.FromKind(kind);
            var pk = Curve.GeneratePrivateKey();
            Store = new PlainSecretStore(pk);
            pk.Flush();
        }

        internal Key(byte[] bytes, ECKind kind, bool flush = false)
        {
            Curve = Curve.FromKind(kind);
            var pk = Curve.ExtractPrivateKey(bytes ?? throw new ArgumentNullException(nameof(bytes)));
            Store = new PlainSecretStore(pk);
            pk.Flush();
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
                return Base58.Convert(Store.Data, Curve.PrivateKeyPrefix);
            }
        }

        public string GetHex()
        {
            using (Store.Unlock())
            {
                return Hex.Convert(Store.Data);
            }
        }

        public Signature Sign(byte[] bytes)
        {
            using (Store.Unlock())
            {
                return Curve.Sign(bytes, Store.Data);
            }
        }

        public Signature Sign(string message)
        {
            using (Store.Unlock())
            {
                return Curve.Sign(Utf8.Parse(message), Store.Data);
            }
        }

        /// <summary>
        /// Prepends forged operation bytes with 0x03 and signs the result
        /// </summary>
        /// <param name="bytes">Forged operation bytes</param>
        /// <returns></returns>
        public Signature SignOperation(byte[] bytes)
        {
            using (Store.Unlock())
            {
                return Curve.Sign(new byte[] { 3 }.Concat(bytes), Store.Data);
            }
        }

        public bool Verify(byte[] data, byte[] signature) => PubKey.Verify(data, signature);

        public bool Verify(string message, string signature) => PubKey.Verify(message, signature);

        public override string ToString() => GetBase58();

        #region static
        public static Key FromBytes(byte[] bytes, ECKind kind = ECKind.Ed25519)
            => new(bytes, kind);

        public static Key FromHex(string hex, ECKind kind = ECKind.Ed25519)
            => new(Hex.Parse(hex), kind, true);

        public static Key FromBase64(string base64, ECKind kind = ECKind.Ed25519)
            => new(Base64.Parse(base64), kind, true);

        public static Key FromBase58(string base58)
        {
            if (base58 == null)
                throw new ArgumentNullException(nameof(base58));

            if (base58.Length != 54 && base58.Length != 98)
                throw new ArgumentException("Invalid private key format. Expected base58 string of 54 or 98 characters.");

            var curve = Curve.FromPrivateKeyBase58(base58);
            var bytes = Base58.Parse(base58, curve.PrivateKeyPrefix);

            return new(bytes, curve.Kind, true);
        }

        public static Key FromMnemonic(Mnemonic mnemonic)
        {
            var seed = mnemonic.GetSeed();
            var key = new Key(seed.GetBytes(0, 32), ECKind.Ed25519, true);
            seed.Flush();
            return key;
        }

        public static Key FromMnemonic(Mnemonic mnemonic, string email, string password)
        {
            var seed = mnemonic.GetSeed($"{email}{password}");
            var key = new Key(seed.GetBytes(0, 32), ECKind.Ed25519, true);
            seed.Flush();
            return key;
        }

        public static Key FromMnemonic(Mnemonic mnemonic, string passphrase, ECKind kind = ECKind.Ed25519)
        {
            var seed = mnemonic.GetSeed(passphrase);
            var key = new Key(seed.GetBytes(0, 32), kind, true);
            seed.Flush();
            return key;
        }
        #endregion
    }
}
