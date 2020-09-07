using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Math;

using Netezos.Keys.Utils.Crypto;
using Netezos.Keys.Utils;

namespace Netezos.Keys.Crypto
{
    class Secp256k1 : ICurve
    {
        #region static
        static readonly BigInteger _Order = new BigInteger(1, Hex.Parse("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEBAAEDCE6AF48A03BBFD25E8CD0364141"));
        static readonly BigInteger _MaxS = new BigInteger(1, Hex.Parse("7FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF5D576E7357A4501DDFE92F46681B20A0"));

        static readonly byte[] _AddressPrefix = { 6, 161, 161 };
        static readonly byte[] _PublicKeyPrefix = { 3, 254, 226, 86 };
        static readonly byte[] _PrivateKeyPrefix = { 17, 162, 224, 201 };
        static readonly byte[] _SignaturePrefix = { 13, 115, 101, 19, 63 };
        #endregion

        public ECKind Kind => ECKind.Secp256k1;

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
            var curve = SecNamedCurves.GetByName("secp256k1");
            var parameters = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H, curve.GetSeed());
            var key = new ECPrivateKeyParameters(new BigInteger(privateKey), parameters);

            var q = key.Parameters.G.Multiply(key.D);
            var publicParams = new ECPublicKeyParameters(q, parameters);

            return publicParams.Q.GetEncoded(true);
        }

        public Signature Sign(byte[] msg, byte[] prvKey)
        {
            var keyedHash = Blake2b.GetDigest(msg);
            var curve = SecNamedCurves.GetByName("secp256k1");
            var parameters = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H, curve.GetSeed());
            var key = new ECPrivateKeyParameters(new BigInteger(prvKey), parameters);
            var signer = new ECDsaSigner(new HMacDsaKCalculator(new Blake2bDigest(256)));
            signer.Init(true, parameters: key);
            var rs = signer.GenerateSignature(keyedHash);

            if (rs[1].CompareTo(_MaxS) > 0)
                rs[1] = _Order.Subtract(rs[1]);
            
            var r = rs[0].ToByteArrayUnsigned();
            var s = rs[1].ToByteArrayUnsigned();
           
            return new Signature(r.Concat(s), _SignaturePrefix);
        }
        
        public bool Verify(byte[] msg, byte[] sig, byte[] pubKey)
        {
            var r = sig.GetBytes(0, 32);
            var s = sig.GetBytes(32, 32);
            var keyedHash = Blake2b.GetDigest(msg);
            
            var curve = SecNamedCurves.GetByName("secp256k1");
            var parameters = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H, curve.GetSeed());
            var key = new ECPublicKeyParameters(curve.Curve.DecodePoint(pubKey), parameters);
            
            var verifier = new ECDsaSigner();
            verifier.Init(false, key);
            
            return verifier.VerifySignature(keyedHash, new BigInteger(1, r), new BigInteger(1, s));
        }
    }
}
