using System;
using System.Collections.Generic;
using System.Text;

using Netezos.Keys.Crypto;

namespace Netezos.Keys
{
    public class PubKey
    {
        readonly ICurve Curve;

        internal PubKey(byte[] bytes, ECKind curve)
        {
        }

        public byte[] GetBytes() => throw new NotImplementedException();
        public string GetAddress() => throw new NotImplementedException();

        public bool Verify(byte[] data, byte[] signature) => throw new NotImplementedException();

        #region static
        public static PubKey FromHex(string hex, ECKind curve) => throw new NotImplementedException();
        public static PubKey FromBase64(string base64, ECKind curve) => throw new NotImplementedException();
        public static PubKey FromBytes(byte[] bytes, ECKind curve) => throw new NotImplementedException();
        #endregion
    }
}
