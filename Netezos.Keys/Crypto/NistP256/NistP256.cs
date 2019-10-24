using System;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Math;

using Netezos.Keys.Utils.Crypto;

namespace Netezos.Keys.Crypto
{
    class NistP256 : ICurve
    {
        #region static
        static readonly byte[] _AddressPrefix = { 6, 161, 164 };
        static readonly byte[] _PublicKeyPrefix = { 3, 178, 139, 127 };
        static readonly byte[] _PrivateKeyPrefix = { 16, 81, 238, 189 };
        static readonly byte[] _SignaturePrefix = { 54, 240, 44, 52 };
        #endregion

        public ECKind Kind => ECKind.NistP256;

        public byte[] AddressPrefix => _AddressPrefix;
        public byte[] PublicKeyPrefix => _PublicKeyPrefix;
        public byte[] PrivateKeyPrefix => _PrivateKeyPrefix;
        public byte[] SignaturePrefix => _SignaturePrefix;

        public byte[] GetPrivateKey(byte[] bytes)
        {
            return bytes.GetBytes(0, 32);
        }

        public byte[] GetPublicKey(byte[] privateKey)
        {
            throw new NotImplementedException();
            //TODO Figure out why it doesn't work with private key p2sk3PM77YMR99AvD3fSSxeLChMdiQ6kkEzqoPuSwQqhPsh29irGLC
            /*            var curve = SecNamedCurves.GetByName("secp256r1");
                        var parameters = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H, curve.GetSeed());
                        var key = new ECPrivateKeyParameters(new BigInteger(privateKey), parameters);

                        var q = key.Parameters.G.Multiply(key.D);

                        return q.GetEncoded(true);*/
        }

        public Signature Sign(byte[] msg, byte[] prvKey)
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
           
            return new Signature(r.Concat(s), _SignaturePrefix);
        }
        
        public bool Verify(byte[] msg, byte[] sig, byte[] pubKey)
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
    }
}
