﻿using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Math;
using Netezos.Utils;

namespace Netezos.Keys
{
    class NistP256 : Curve
    {
        public override ECKind Kind => ECKind.NistP256;

        public override byte[] AddressPrefix => Prefix.tz3;
        public override byte[] PublicKeyPrefix => Prefix.p2pk;
        public override byte[] PrivateKeyPrefix => Prefix.p2sk;
        public override byte[] SignaturePrefix => Prefix.p2sig;
        public override byte[] GenericSignaturePrefix => Prefix.sig;

        public override byte[] GeneratePrivateKey()
        {
            var curve = SecNamedCurves.GetByName("secp256r1");
            byte[] res = new byte[32];

            do { RNG.WriteBytes(res); }
            while (new BigInteger(1, res).CompareTo(curve.N) >= 0);

            return res;
        }

        public override byte[] GetPublicKey(byte[] privateKey)
        {
            var curve = SecNamedCurves.GetByName("secp256r1");
            var parameters = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H, curve.GetSeed());
            var key = new ECPrivateKeyParameters(new BigInteger(1, privateKey), parameters);

            var q = key.Parameters.G.Multiply(key.D);
            return q.GetEncoded(true);
        }

        public override Signature Sign(byte[] msg, byte[] prvKey, bool generic = false)
        {
            var curve = SecNamedCurves.GetByName("secp256r1");
            var parameters = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H, curve.GetSeed());
            var privateKey = new ECPrivateKeyParameters(new BigInteger(1, prvKey), parameters);
            var signer = new ECDsaSigner(new HMacDsaKCalculator(new Blake2bDigest(256)));

            signer.Init(true, privateKey);
            var rs = signer.GenerateSignature(Blake2b.GetDigest(msg));

            if (rs[1].CompareTo(curve.N.Divide(BigInteger.Two)) > 0)
                rs[1] = curve.N.Subtract(rs[1]);

            var r = rs[0].ToByteArrayUnsigned().Align(32);
            var s = rs[1].ToByteArrayUnsigned().Align(32);

            return new Signature(r.Concat(s), generic ? GenericSignaturePrefix : SignaturePrefix);
        }

        public override bool Verify(byte[] msg, byte[] sig, byte[] pubKey)
        {
            var digest = Blake2b.GetDigest(msg);
            var r = sig.GetBytes(0, 32);
            var s = sig.GetBytes(32, 32);

            var curve = SecNamedCurves.GetByName("secp256r1");
            var parameters = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H, curve.GetSeed());
            var publicKey = new ECPublicKeyParameters(curve.Curve.DecodePoint(pubKey), parameters);
            var signer = new ECDsaSigner();

            signer.Init(false, publicKey);
            return signer.VerifySignature(digest, new BigInteger(1, r), new BigInteger(1, s));
        }
    }
}
