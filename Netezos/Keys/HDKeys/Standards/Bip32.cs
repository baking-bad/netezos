using System;
using System.Collections.Generic;
using System.Text;

namespace Netezos.Keys
{
    class Bip32 : HDStandard
    {
        public override HDStandardKind Kind => HDStandardKind.Bip32;

        public override byte[] GenerateMasterKey(Curve curve, byte[] seed)
        {
            throw new NotImplementedException();
        }

        public override byte[] GetChildPrivateKey(Curve curve, byte[] extKey, uint index)
        {
            throw new NotImplementedException();
        }

        public override byte[] GetChildPublicKey(Curve curve, byte[] extKey, uint index)
        {
            throw new NotImplementedException();
        }
    }
}
