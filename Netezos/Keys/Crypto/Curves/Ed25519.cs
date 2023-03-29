using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using BcEd25519 = Org.BouncyCastle.Math.EC.Rfc8032.Ed25519;
using Netezos.Utils;

namespace Netezos.Keys
{
    class Ed25519 : Curve
    {
        #region static
        static readonly byte[] _Slip10Seed = { 101, 100, 50, 53, 53, 49, 57, 32, 115, 101, 101, 100 }; // "ed25519 seed"
        #endregion
        
        public override ECKind Kind => ECKind.Ed25519;

        public override byte[] AddressPrefix => Prefix.tz1;
        public override byte[] PublicKeyPrefix => Prefix.edpk;
        public override byte[] PrivateKeyPrefix => Prefix.edsk;
        public override byte[] SignaturePrefix => Prefix.edsig;
        public override byte[] Slip10Seed => _Slip10Seed;

        public override byte[] ExtractPrivateKey(byte[] bytes)
        {
            if (bytes.Length != 32 && (bytes.Length != 64 || !bytes.IsEqual(32, GetPublicKey(bytes))))
                throw new ArgumentException("Invalid private key format. Expected 32 bytes seed, or 64 bytes seed+pubkey.");

            return bytes.GetBytes(0, 32);
        }

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

            return new Signature(signer.GenerateSignature(), SignaturePrefix);
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
