using Netezos.Keys.Utils.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;

namespace Netezos.Keys.Crypto
{
    class Ed25519 : ICurve
    {
        public ECKind Kind => ECKind.Ed25519;

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
    }
}
