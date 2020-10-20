using System;
using System.Collections.Generic;
using System.Text;

namespace Netezos.Keys
{
    class Slip10 : HDStandard
    {
        public override HDStandardKind Kind => HDStandardKind.Slip10;

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
