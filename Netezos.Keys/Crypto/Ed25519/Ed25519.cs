using Netezos.Keys.Utils.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using BcEd25519 = Org.BouncyCastle.Math.EC.Rfc8032.Ed25519;


namespace Netezos.Keys.Crypto
{
    public class Ed25519 : ICurve
    {
        public ECKind Kind => ECKind.Ed25519;

        #region static
        static readonly byte[] _AddressPrefix = { 6, 161, 159 };
        static readonly byte[] _PublicKeyPrefix = { 13, 15, 37, 217 };
        static readonly byte[] _PrivateKeyPrefix = { 43, 246, 78, 7 };
        static readonly byte[] _SignaturePrefix = { 9, 245, 205, 134, 18 };
        
        #endregion

        public byte[] AddressPrefix => _AddressPrefix;
        public byte[] PublicKeyPrefix => _PublicKeyPrefix;
        public byte[] PrivateKeyPrefix => _PrivateKeyPrefix;
        public byte[] SignaturePrefix => _SignaturePrefix;

        const int PublicKeySize = 32;
        const int PrivateKeySize = 32;

        public Signature Sign(byte[] prvKey, byte[] msg)
        {
            var keyedHash = Blake2b.GetDigest(msg);
            var privateKey = new Ed25519PrivateKeyParameters(prvKey, 0);
            var signer = new Ed25519Signer();
            signer.Init(true, privateKey);
            signer.BlockUpdate(keyedHash, 0, keyedHash.Length);
            
            return new Signature(signer.GenerateSignature(), _SignaturePrefix);
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
