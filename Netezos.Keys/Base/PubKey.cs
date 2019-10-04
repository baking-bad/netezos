using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
        public string GetBase58() => Base58.Convert(PublicKey, Curve.PublicKeyPrefix);
        public bool Verify(byte[] data, byte[] signature) => Curve.Verify(PublicKey, data, signature);

        #region static

        public static PubKey FromHex(string hex, ECKind curve) => new PubKey(Hex.Parse(hex), curve);
        public static PubKey FromBase64(string base64, ECKind curve) => throw new NotImplementedException();
        public static PubKey FromBase58(string base58, ECKind curve) => new PubKey(Base58.Parse(base58, Crypto.Curve.GetCurve(curve).PublicKeyPrefix), curve);
        public static PubKey FromBytes(byte[] bytes, ECKind curve) => new PubKey(bytes, curve);
        #endregion
    }
}
