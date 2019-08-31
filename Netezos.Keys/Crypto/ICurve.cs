namespace Netezos.Keys.Crypto
{
    interface ICurve
    {
        ECKind Kind { get; }

        byte[] Sign(byte[] prvKey, byte[] msg);
        bool Verify(byte[] pubKey, byte[] msg, byte[] sig);
    }
}
