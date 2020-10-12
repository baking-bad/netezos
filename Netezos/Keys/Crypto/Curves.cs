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
                case "tz1":
                case "edesk":
                case "edsig":
                    return new Ed25519();
                case "sppk":
                case "spsk":
                case "tz2":
                case "spesk":
                case "spsig":
                    return new Secp256k1();
                case "p2pk":
                case "p2sk":
                case "tz3":
                case "p2esk":
                case "p2sig":
                    return new NistP256();
                default:
                    throw new ArgumentException("Invalid prefix");
            }
        }
    }
}
