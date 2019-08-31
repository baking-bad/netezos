using System;
using System.Collections.Generic;
using System.Text;

namespace Netezos.Keys.Crypto
{
    class Secp256k1 : ICurve
    {
        public ECKind Kind => ECKind.Secp256k1;

        public byte[] Sign(byte[] prvKey, byte[] msg) => throw new NotImplementedException();
        public bool Verify(byte[] pubKey, byte[] msg, byte[] sig) => throw new NotImplementedException();
    }
}
