using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Math;
using Netezos.Utils;

namespace Netezos.Keys
{
    class Secp256k1 : Curve
    {
        #region static
        static readonly byte[] _AddressPrefix = { 6, 161, 161 };
        static readonly byte[] _PublicKeyPrefix = { 3, 254, 226, 86 };
        static readonly byte[] _PrivateKeyPrefix = { 17, 162, 224, 201 };
        static readonly byte[] _SignaturePrefix = { 13, 115, 101, 19, 63 };
        static readonly byte[] _SeedKey = { 66, 105, 116, 99, 111, 105, 110, 32, 115, 101, 101, 100 }; // "Bitcoin seed"
        #endregion

        public override ECKind Kind => ECKind.Secp256k1;

        public override byte[] AddressPrefix => _AddressPrefix;
        public override byte[] PublicKeyPrefix => _PublicKeyPrefix;
        public override byte[] PrivateKeyPrefix => _PrivateKeyPrefix;
        public override byte[] SignaturePrefix => _SignaturePrefix;
        public override byte[] SeedKey => _SeedKey;

        public override byte[] GeneratePrivateKey()
        {
            var curve = SecNamedCurves.GetByName("secp256k1");
            byte[] res = new byte[32];

            do { RNG.WriteBytes(res); }
            while (new BigInteger(1, res).CompareTo(curve.N) >= 0);

            return res;
        }

        public override byte[] GetPublicKey(byte[] privateKey)
        {
            var curve = SecNamedCurves.GetByName("secp256k1");
            var parameters = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H, curve.GetSeed());
            var key = new ECPrivateKeyParameters(new BigInteger(1, privateKey), parameters);

            var q = key.Parameters.G.Multiply(key.D);
            return q.GetEncoded(true);
        }

        public override Signature Sign(byte[] msg, byte[] prvKey)
        {
            var curve = SecNamedCurves.GetByName("secp256k1");
            var parameters = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H, curve.GetSeed());
            var privateKey = new ECPrivateKeyParameters(new BigInteger(1, prvKey), parameters);
            var signer = new ECDsaSigner(new HMacDsaKCalculator(new Blake2bDigest(256)));

            signer.Init(true, privateKey);
            var rs = signer.GenerateSignature(Blake2b.GetDigest(msg));

            if (rs[1].CompareTo(curve.N.Divide(BigInteger.Two)) > 0)
                rs[1] = curve.N.Subtract(rs[1]);

            var r = rs[0].ToByteArrayUnsigned().Align(32);
            var s = rs[1].ToByteArrayUnsigned().Align(32);

            return new Signature(r.Concat(s), _SignaturePrefix);
        }

        public override bool Verify(byte[] msg, byte[] sig, byte[] pubKey)
        {
            var digest = Blake2b.GetDigest(msg);
            var r = sig.GetBytes(0, 32);
            var s = sig.GetBytes(32, 32);

            var curve = SecNamedCurves.GetByName("secp256k1");
            var parameters = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H, curve.GetSeed());
            var publicKey = new ECPublicKeyParameters(curve.Curve.DecodePoint(pubKey), parameters);
            var signer = new ECDsaSigner();

            signer.Init(false, publicKey);
            return signer.VerifySignature(digest, new BigInteger(1, r), new BigInteger(1, s));
        }
    }
}
