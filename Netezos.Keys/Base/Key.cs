using System;
using System.Security;
using System.Text;
using Org.BouncyCastle.Utilities.Encoders;

using Netezos.Keys.Crypto;

namespace Netezos.Keys
{
    public class Key
    {
        readonly ICurve Curve;
        readonly ISecretStore Store;
        readonly PubKey PubKey;

        public Key() : this(new Mnemonic()) {}

        public Key(ECKind curve) : this(new Mnemonic(), "", curve) {}

        public Key(Mnemonic mnemonic, SecureString passphrase) : this(mnemonic, passphrase.Unsecure()) {}

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
        public Key(Mnemonic mnemonic, string passphrase, ECKind curve)
        {
            Curve = Crypto.Curve.GetCurve(curve);
            var seed = mnemonic.GetSeed(passphrase);
            var privateKey = Curve.GetPrivateKey(seed);
            Store = new PlainSecretStore(Curve.GetPrivateKey(seed));
            PubKey = new PubKey(Curve.GetPublicKey(privateKey), Curve.Kind);
            seed.Reset();
            privateKey.Reset();
        }
        
        public Key(byte[] privateKey, ECKind curve)
        {
            Curve = Crypto.Curve.GetCurve(curve);
            Store = new PlainSecretStore(privateKey);
            PubKey = new PubKey(Curve.GetPublicKey(privateKey), Curve.Kind);
            privateKey.Reset();
        }
        

        public string GetAddress() => PubKey.GetAddress();
        public PubKey GetPublicKey() => PubKey;

        public Signature Sign(byte[] bytes)
        {
            using (Store)
            {
                return Curve.Sign(Store.Data, bytes);
            }
        }
        public Signature Sign(string message) => Sign(Encoding.UTF8.GetBytes(message));

        public bool Verify(byte[] data, byte[] signature) => PubKey.Verify(data, signature);

        public bool Verify(string message, string signature) =>
            PubKey.Verify(Encoding.UTF8.GetBytes(message), Base58.Parse(signature, Curve.SignaturePrefix));

        #region static
        public static Key FromHex(string hex, ECKind curve) => FromBytes(Hex.Parse(hex), curve);
        public static Key FromBase64(string base64, ECKind curve) => FromBytes(Base64.Decode(base64), curve);
        public static Key FromBase58(string base58)
        {
            var curve = GetCurveFromPrefix(base58.Substring(0, 4));
            var bytes = Base58.Parse(base58, Crypto.Curve.GetCurve(curve).PrivateKeyPrefix);
            return FromBytes(bytes, curve);
        }
        public static Key FromBytes(byte[] bytes, ECKind curve) => new Key(bytes, curve);
        #endregion

        static ECKind GetCurveFromPrefix(string prefix)
        {
            switch (prefix)
            {
            case "edsk":
                return ECKind.Ed25519;
            case "spsk":
                return ECKind.Secp256k1;
            case "p2sk":
                return ECKind.NistP256;
            default:
                throw new ArgumentException();
            }
        }
    }
}
