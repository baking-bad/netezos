using System;
using Netezos.Keys.Utils.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using BcEd25519 = Org.BouncyCastle.Math.EC.Rfc8032.Ed25519;

namespace Netezos.Keys.Crypto
{
    class Ed25519 : ICurve
    {
        #region static
        static readonly byte[] _AddressPrefix = { 6, 161, 159 };
        static readonly byte[] _PublicKeyPrefix = { 13, 15, 37, 217 };
        static readonly byte[] _PrivateKeyPrefix = { 43, 246, 78, 7 };
        static readonly byte[] _SignaturePrefix = { 9, 245, 205, 134, 18 };
        #endregion

        public ECKind Kind => ECKind.Ed25519;

        public byte[] AddressPrefix => _AddressPrefix;
        public byte[] PublicKeyPrefix => _PublicKeyPrefix;
        public byte[] PrivateKeyPrefix => _PrivateKeyPrefix;
        public byte[] SignaturePrefix => _SignaturePrefix;

        public byte[] GetPrivateKey(byte[] bytes)
        {
            var result = new byte[64];
            Buffer.BlockCopy(bytes, 0, result, 0, 32);
            BcEd25519.GeneratePublicKey(result, 0, result, 32);

            return result;
        }

        public byte[] GetPublicKey(byte[] privateKey)
        {
            var publicKey = new byte[32];
            BcEd25519.GeneratePublicKey(privateKey, 0, publicKey, 0);

            return publicKey;
        }

        public Signature Sign(byte[] msg, byte[] prvKey)
        {
            var digest = Blake2b.GetDigest(msg);
            var privateKey = new Ed25519PrivateKeyParameters(prvKey, 0);
            var signer = new Ed25519Signer();

            signer.Init(true, privateKey);
            signer.BlockUpdate(digest, 0, digest.Length);
            
            return new Signature(signer.GenerateSignature(), _SignaturePrefix);
        }

        public bool Verify(byte[] msg, byte[] sig, byte[] pubKey)
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
