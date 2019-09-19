using System;
using System.Collections.Generic;
using System.Text;

using Netezos.Keys.Crypto;
using Netezos.Keys.Utils.Crypto;

namespace Netezos.Keys
{
    public class PubKey
    {
        readonly ICurve Curve;
        readonly byte[] PublicKey;

        internal PubKey(byte[] bytes, ECKind curve)
        {
            PublicKey = bytes;
            
            Curve = Crypto.Curve.GetCurve(curve);
        }

        public byte[] GetBytes() => PublicKey;
        public string GetAddress() => Base58.Convert(Blake2b.GetDigest(PublicKey, 160), Curve.AddressPrefix);
        public bool Verify(byte[] data, byte[] signature) => Curve.Verify(PublicKey, data, signature);

        #region static
        public static PubKey FromHex(string hex, ECKind curve) => throw new NotImplementedException();
        public static PubKey FromBase64(string base64, ECKind curve) => throw new NotImplementedException();
        public static PubKey FromBytes(byte[] bytes, ECKind curve) => throw new NotImplementedException();
        #endregion
    }
}
