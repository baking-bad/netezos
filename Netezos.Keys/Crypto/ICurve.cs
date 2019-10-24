namespace Netezos.Keys.Crypto
{
    interface ICurve
    {
        ECKind Kind { get; }
        
        byte[] AddressPrefix { get; }
        byte[] PublicKeyPrefix { get; }
        byte[] PrivateKeyPrefix { get; }
        byte[] SignaturePrefix { get; }

        byte[] GetPrivateKey(byte[] bytes);

        byte[] GetPublicKey(byte[] privateKey);

        Signature Sign(byte[] bytes, byte[] prvKey);

        bool Verify(byte[] bytes, byte[] signature, byte[] pubKey);
    }
}
