using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

using Netezos.Keys.Crypto;
using Netezos.Keys.Utils.Crypto;

namespace Netezos.Keys
{
    public class Key
    {
        readonly ICurve Curve;
        readonly ISecretStore Store;
        readonly PubKey PubKey;

        public Key() : this(new Mnemonic()) {}
        public Key(ECKind curve)
        {
        }

        public Key(Mnemonic mnemonic, SecureString passphrase) => throw new NotImplementedException();

        public Key(Mnemonic mnemonic, string passphrase = "")
        {
            Curve = new Ed25519();
            
            var seed = mnemonic.GetSeed(passphrase);
            var privateKey = Curve.GetPrivateKey(seed);
            Store = new PlainSecretStore(Curve.GetPrivateKey(seed));
            PubKey = new PubKey(Curve.GetPublicKey(privateKey), Curve.Kind);
            seed.Reset();
            privateKey.Reset();
        }
        public Key(Mnemonic mnemonic, string email, string password) : this(mnemonic, $"{email}{password}") {}
        public Key(Mnemonic mnemonic, ECKind curve)
        {
        }

        public string GetAddress() => PubKey.GetAddress();
        public PubKey GetPublicKey() => PubKey;

        public byte[] Sign(byte[] bytes)
        {
            using (Store)
            {
                return Curve.Sign(Store.Data, bytes);
            }
        }
        public byte[] Sign(string message) => Sign(Encoding.UTF8.GetBytes(message));

        public bool Verify(byte[] data, byte[] signature) => PubKey.Verify(data, signature);

        public bool Verify(string message, string signature) =>
            PubKey.Verify(Encoding.UTF8.GetBytes(message), Base58.Parse(signature, Curve.SignaturePrefix));

        #region static
        public static Key FromHex(string hex, ECKind curve) => throw new NotImplementedException();
        public static Key FromBase64(string base64, ECKind curve) => throw new NotImplementedException();
        public static Key FromBase58(string base58) => throw new NotImplementedException();
        public static Key FromBytes(byte[] bytes, ECKind curve) => throw new NotImplementedException();
        #endregion
    }
}
