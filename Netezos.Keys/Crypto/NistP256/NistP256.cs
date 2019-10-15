using System;
using Netezos.Keys.Utils.Crypto;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Math;

namespace Netezos.Keys.Crypto
{
    public class NistP256 : ICurve
    {
        public ECKind Kind => ECKind.NistP256;
        
        static readonly byte[] _AddressPrefix = { 6, 161, 164 };
        static readonly byte[] _PublicKeyPrefix = { 3, 178, 139, 127 };
        static readonly byte[] _PrivateKeyPrefix = { 16, 81, 238, 189 };
        static readonly byte[] _SignaturePrefix = { 54, 240, 44, 52 };
        
        public byte[] AddressPrefix => _AddressPrefix;
        public byte[] PublicKeyPrefix => _PublicKeyPrefix;
        public byte[] PrivateKeyPrefix => _PrivateKeyPrefix;
        public byte[] SignaturePrefix => _SignaturePrefix;

        public Signature Sign(byte[] prvKey, byte[] msg)
        {
            var keyedHash = Blake2b.GetDigest(msg);
            var curve = SecNamedCurves.GetByName("secp256r1");
            var parameters = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H, curve.GetSeed());
            var key = new ECPrivateKeyParameters(new BigInteger(prvKey), parameters);
            var signer = new ECDsaSigner(new HMacDsaKCalculator(new Blake2bDigest(256)));
            signer.Init(true, parameters: key);
            var rs = signer.GenerateSignature(keyedHash);
            
            var r = rs[0].ToByteArrayUnsigned();
            var s = rs[1].ToByteArrayUnsigned();
           
            return new Signature(r.Concat(s), Kind);
        }
        
        public bool Verify(byte[] pubKey, byte[] msg, byte[] sig)
        {
            var r = sig.GetBytes(0, 32);
            var s = sig.GetBytes(32, 32);
            var keyedHash = Blake2b.GetDigest(msg);
            
            var curve = SecNamedCurves.GetByName("secp256r1");
            var parameters = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H, curve.GetSeed());
            var key = new ECPublicKeyParameters(curve.Curve.DecodePoint(pubKey), parameters);
            
            var verifier = new ECDsaSigner();
            verifier.Init(false, key);
            
            return verifier.VerifySignature(keyedHash, new BigInteger(1, r), new BigInteger(1, s));
        }

        public byte[] GetPrivateKey(byte[] seed)
        {
            return seed;
        }

        public byte[] GetPublicKey(byte[] privateKey)
        {
            var curve = SecNamedCurves.GetByName("secp256r1");
            var parameters = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H, curve.GetSeed());
            var key = new ECPrivateKeyParameters(new BigInteger(privateKey), parameters);
            
            var q = key.Parameters.G.Multiply(key.D);

            return q.GetEncoded(true);
        }
    }
}
