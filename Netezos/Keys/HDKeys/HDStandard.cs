using System;

namespace Netezos.Keys
{
    abstract class HDStandard
    {
        public abstract HDStandardKind Kind { get; }

        public abstract (byte[], byte[]) GenerateMasterKey(Curve curve, byte[] seed);

        public abstract (byte[], byte[]) GetChildPrivateKey(Curve curve, byte[] privateKey, byte[] chainCode, uint index);

        #region static
        public static HDStandard FromKind(HDStandardKind kind)
        {
            return kind switch
            {
                HDStandardKind.Slip10 => new Slip10(),
                _ => throw new ArgumentException("Invalid HD standard")
            };
        }
        #endregion

        public abstract (byte[], byte[]) GetChildPublicKey(Curve curve, byte[] pubKey, byte[] chainCode, uint index);
    }

    public enum HDStandardKind
    {
        // Bip32, //https://github.com/bitcoin/bips/blob/master/bip-0032.mediawiki
        Slip10, //https://github.com/satoshilabs/slips/blob/master/slip-0010.md
    }
}
