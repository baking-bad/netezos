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
        public abstract byte[] SeedKey { get; }

        public abstract byte[] GeneratePrivateKey();

        public abstract byte[] GetPublicKey(byte[] privateKey);

        public abstract Signature Sign(byte[] bytes, byte[] prvKey);

        public abstract bool Verify(byte[] bytes, byte[] signature, byte[] pubKey);

        #region static
        public static Curve FromKind(ECKind kind) => kind switch
        {
            ECKind.Ed25519 => new Ed25519(),
            ECKind.Secp256k1 => new Secp256k1(),
            ECKind.NistP256 => new NistP256(),
            ECKind.Bls12381 => new Bls12381(),
            _ => throw new ArgumentException("Invalid EC kind")
        };

        public static Curve FromPrefix(string prefix) => prefix switch
        {
            "edpk" or "edsk" or "tz1" or "edesk" or "edsig" => new Ed25519(),
            "sppk" or "spsk" or "tz2" or "spesk" or "spsig" => new Secp256k1(),
            "p2pk" or "p2sk" or "tz3" or "p2esk" or "p2sig" => new NistP256(),
            "BLpk" or "BLsk" or "tz4" or "BLesk" or "BLsig" => new Bls12381(),
            _ => throw new ArgumentException("Invalid prefix"),
        };
        #endregion
    }
}
