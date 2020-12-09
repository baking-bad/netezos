using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using BcEd25519 = Org.BouncyCastle.Math.EC.Rfc8032.Ed25519;
using Netezos.Utils;

namespace Netezos.Keys
{
    class Ed25519 : Curve
    {
        #region static
        static readonly byte[] _AddressPrefix = { 6, 161, 159 };
        static readonly byte[] _PublicKeyPrefix = { 13, 15, 37, 217 };
        static readonly byte[] _PrivateKeyPrefix = { 13, 15, 58, 7 };
        static readonly byte[] _SignaturePrefix = { 9, 245, 205, 134, 18 };
        #endregion

        public override ECKind Kind => ECKind.Ed25519;

        public override byte[] AddressPrefix => _AddressPrefix;
        public override byte[] PublicKeyPrefix => _PublicKeyPrefix;
        public override byte[] PrivateKeyPrefix => _PrivateKeyPrefix;
        public override byte[] SignaturePrefix => _SignaturePrefix;

        public override byte[] GeneratePrivateKey()
        {
            return RNG.GetBytes(32);
        }

        public override byte[] GetPublicKey(byte[] privateKey)
        {
            var publicKey = new byte[32];
            BcEd25519.GeneratePublicKey(privateKey, 0, publicKey, 0);

            return publicKey;
        }

        public override Signature Sign(byte[] msg, byte[] prvKey)
        {
            var digest = Blake2b.GetDigest(msg);
            var privateKey = new Ed25519PrivateKeyParameters(prvKey, 0);
            var signer = new Ed25519Signer();

            signer.Init(true, privateKey);
            signer.BlockUpdate(digest, 0, digest.Length);

            return new Signature(signer.GenerateSignature(), _SignaturePrefix);
        }

        public override bool Verify(byte[] msg, byte[] sig, byte[] pubKey)
        {
            var digest = Blake2b.GetDigest(msg);
            var publicKey = new Ed25519PublicKeyParameters(pubKey, 0);
            var verifier = new Ed25519Signer();

            verifier.Init(false, publicKey);
            verifier.BlockUpdate(digest, 0, digest.Length);

            return verifier.VerifySignature(sig);
        }
    }
}
