using System;

namespace Netezos.Keys
{
    abstract class Curve
    {
        public abstract ECKind Kind { get; }

        public abstract byte[] AddressPrefix { get; }
        public abstract byte[] PublicKeyPrefix { get; }
        public abstract byte[] PrivateKeyPrefix { get; }
        public abstract byte[] SignaturePrefix { get; }

        public abstract byte[] GeneratePrivateKey();

        public abstract byte[] GetPublicKey(byte[] privateKey);

        public abstract Signature Sign(byte[] bytes, byte[] prvKey);

        public abstract bool Verify(byte[] bytes, byte[] signature, byte[] pubKey);

        #region static
        public static Curve FromKind(ECKind kind)
        {
            return kind == ECKind.Ed25519
                ? new Ed25519()
                : kind == ECKind.NistP256
                    ? (Curve)new NistP256()
                    : new Secp256k1();
        }

        public static Curve FromPrefix(string prefix)
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
        #endregion
    }
}
