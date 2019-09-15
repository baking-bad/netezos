using Netezos.Keys.Utils.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using BcEd25519 = Org.BouncyCastle.Math.EC.Rfc8032.Ed25519;


namespace Netezos.Keys.Crypto
{
    public class Ed25519 : ICurve
    {
        public ECKind Kind => ECKind.Ed25519;
        
        public const int PublicKeySize = 32;
        public const int PrivateKeySize = 32;
        public const int ExtendedPrivateKeySize = 64;

        public byte[] Sign(byte[] prvKey, byte[] msg)
        {
            var keyedHash = Blake2b.GetDigest(msg);
            var privateKey = new Ed25519PrivateKeyParameters(prvKey, 0);
            var signer = new Ed25519Signer();
            signer.Init(true, privateKey);
            signer.BlockUpdate(keyedHash, 0, keyedHash.Length);
            
            return signer.GenerateSignature();
        }
        public bool Verify(byte[] pubKey, byte[] msg, byte[] sig)
        {
            var keyedHash = Blake2b.GetDigest(msg);
            var publicKey = new Ed25519PublicKeyParameters(pubKey, 0);
            var verifier = new Ed25519Signer();
            verifier.Init(false, publicKey);
            verifier.BlockUpdate(keyedHash, 0, keyedHash.Length);
            
            return verifier.VerifySignature(sig);
        }

        public byte[] GetPrivateKey(byte[] seed)
        {
            var privateKey = seed.GetBytes(0, PrivateKeySize);
            var publicKey = new byte[PublicKeySize];

            BcEd25519.GeneratePublicKey(seed, 0, publicKey, 0);

//            return seed.GetBytes(0, PrivateKeySize);
            return privateKey.Concat(publicKey);
        }

        public byte[] GetPublicKey(byte[] privateKey)
        {
            var publicKey = new byte[PublicKeySize];

            BcEd25519.GeneratePublicKey(privateKey, 0, publicKey, 0);
            
            return publicKey;
        }
        
    }
}
