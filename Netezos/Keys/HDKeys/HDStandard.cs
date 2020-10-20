using System;

namespace Netezos.Keys
{
    abstract class HDStandard
    {
        public abstract HDStandardKind Kind { get; }

        public abstract byte[] GenerateMasterKey(Curve curve, byte[] seed);

        public abstract byte[] GetChildPrivateKey(Curve curve, byte[] extKey, uint index);

        public abstract byte[] GetChildPublicKey(Curve curve, byte[] extKey, uint index);

        #region static
        public static HDStandard FromKind(HDStandardKind kind)
        {
            switch (kind)
            {
                case HDStandardKind.Bip32: return new Bip32();
                case HDStandardKind.Slip10: return new Slip10();
                default:
                    throw new ArgumentException("Invalid HD standard");
            }
        }
        #endregion
    }

    public enum HDStandardKind
    {
        Bip32, //https://github.com/bitcoin/bips/blob/master/bip-0032.mediawiki
        Slip10, //https://github.com/satoshilabs/slips/blob/master/slip-0010.md
        //TODO: add custom implemetations from the most popular Tezos wallets for better importing/exporting UX
    }
}
