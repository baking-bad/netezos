namespace Netezos.Keys.Crypto
{
    interface ICurve
    {
        ECKind Kind { get; }
        
        byte[] AddressPrefix { get; }
        byte[] PublicKeyPrefix { get; }
        byte[] PrivateKeyPrefix { get; }
        byte[] SignaturePrefix { get; }

        byte[] Sign(byte[] prvKey, byte[] msg);
        bool Verify(byte[] pubKey, byte[] msg, byte[] sig);
        byte[] GetPrivateKey(byte[] seed);
        byte[] GetPublicKey(byte[] privateKey);
    }

    internal static class Curve
    {
        public static ICurve GetCurve(ECKind curve)
        {
            return curve == ECKind.Ed25519 
                ? new Ed25519() 
                : curve == ECKind.NistP256 
                    ? (ICurve) new NistP256() 
                    : new Secp256k1();
        }
    }
}
