using System;
using System.Collections.Generic;
using System.Text;

namespace Netezos.Keys.Crypto
{
    class Ed25519 : ICurve
    {
        public ECKind Kind => ECKind.Ed25519;

        public byte[] Sign(byte[] prvKey, byte[] msg) => throw new NotImplementedException();
        public bool Verify(byte[] pubKey, byte[] msg, byte[] sig) => throw new NotImplementedException();
    }
}
