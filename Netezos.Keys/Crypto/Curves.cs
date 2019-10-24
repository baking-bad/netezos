using System;

namespace Netezos.Keys.Crypto
{
    static class Curves
    {
        public static ICurve GetCurve(ECKind curve)
        {
            return curve == ECKind.Ed25519
                ? new Ed25519()
                : curve == ECKind.NistP256
                    ? (ICurve)new NistP256()
                    : new Secp256k1();
        }

        public static ICurve GetCurve(string prefix)
        {
            switch (prefix)
            {
                case "edsk":
                    return new Ed25519();
                case "spsk":
                    return new Secp256k1();
                case "p2sk":
                    return new NistP256();
                default:
                    throw new ArgumentException("Invalid prefix");
            }
        }
    }
}
