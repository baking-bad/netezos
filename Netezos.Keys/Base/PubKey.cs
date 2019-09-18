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
            
            Curve = curve == ECKind.Ed25519
                ? new Ed25519()
                : curve == ECKind.Secp256k1
                    ? (ICurve) new Secp256k1()
                    : new NistP256();
        }

        public byte[] GetBytes() => PublicKey;
        public string GetAddress() => Base58.Encode(Blake2b.GetDigest(PublicKey, 160), Prefix.tz1);
        public bool Verify(byte[] data, byte[] signature) => Curve.Verify(PublicKey, data, signature);

        #region static
        public static PubKey FromHex(string hex, ECKind curve) => throw new NotImplementedException();
        public static PubKey FromBase64(string base64, ECKind curve) => throw new NotImplementedException();
        public static PubKey FromBytes(byte[] bytes, ECKind curve) => throw new NotImplementedException();
        #endregion
    }
}
