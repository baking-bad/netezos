using System;

namespace Netezos.Keys.Crypto
{
    static class Curves
    {
        public static ICurve GetCurve(ECKind kind)
        {
            return kind == ECKind.Ed25519
                ? new Ed25519()
                : kind == ECKind.NistP256
                    ? (ICurve)new NistP256()
                    : new Secp256k1();
        }

        public static ICurve GetCurve(string prefix)
        {
            switch (prefix)
            {
                case "edpk":
                case "edsk":
                    return new Ed25519();
                case "sppk":
                case "spsk":
                    return new Secp256k1();
                case "p2pk":
                case "p2sk":
                    return new NistP256();
                default:
                    throw new ArgumentException("Invalid prefix");
            }
        }
    }
}
