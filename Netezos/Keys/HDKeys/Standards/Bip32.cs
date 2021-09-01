using System;
using System.Security.Cryptography;

namespace Netezos.Keys
{
    class Bip32 : HDStandard
    {
        public override HDStandardKind Kind => HDStandardKind.Bip32;

        public override (byte[], byte[]) GenerateMasterKey(Curve curve, byte[] seed)
        {
            throw new NotImplementedException();
        }

        public override (byte[], byte[]) GetChildPrivateKey(Curve curve, byte[] privateKey, byte[] chainCode, uint index)
        {
            throw new NotImplementedException();
        }

        public override (byte[], byte[]) GetChildPublicKey(Curve curve, byte[] extPubKey, byte[] chainCode, uint index)
        {
            throw new NotImplementedException();
        }
    }
}
