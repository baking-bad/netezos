namespace Netezos.Keys
{
    abstract class HDStandard
    {
        #region static
        public static HDStandard Slip10 { get; } = new Slip10();
        public static HDStandard Eip2333 { get; } = new Eip2333();

        public static HDStandard FromCurve(Curve curve) => curve is Bls12381 ? Eip2333 : Slip10;
        public static HDStandard FromECKind(ECKind kind) => kind == ECKind.Bls12381 ? Eip2333 : Slip10;
        #endregion

        public abstract (byte[], byte[]) GenerateMasterKey(Curve curve, byte[] seed);

        public abstract (byte[], byte[]) GetChildPrivateKey(Curve curve, byte[] privateKey, byte[] chainCode, uint index);

        public abstract (byte[], byte[]) GetChildPublicKey(Curve curve, byte[] pubKey, byte[] chainCode, uint index);
    }
}
