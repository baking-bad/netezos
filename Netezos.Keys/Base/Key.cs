using System;
using System.Collections.Generic;
using System.Text;

using Netezos.Keys.Crypto;

namespace Netezos.Keys
{
    public class Key
    {
        readonly ICurve Curve;
        readonly ISecretStore Store;

        public Key()
        {
        }
        public Key(ECKind curve)
        {
        }
        public Key(Mnemonic mnemonic)
        {
        }
        public Key(Mnemonic mnemonic, string passphrase)
        {
        }
        public Key(Mnemonic mnemonic, string email, string password)
        {
        }
        public Key(Mnemonic mnemonic, ECKind curve)
        {
        }

        public string GetAddress() => throw new NotImplementedException();
        public PubKey GetPublicKey() => throw new NotImplementedException();

        public byte[] Sign(byte[] bytes) => throw new NotImplementedException();
        public byte[] Sign(string message) => throw new NotImplementedException();

        public bool Verify(byte[] data, byte[] signature) => throw new NotImplementedException();

        #region static
        public static Key FromHex(string hex, ECKind curve) => throw new NotImplementedException();
        public static Key FromBase64(string base64, ECKind curve) => throw new NotImplementedException();
        public static Key FromBase58(string base58) => throw new NotImplementedException();
        public static Key FromBytes(byte[] bytes, ECKind curve) => throw new NotImplementedException();
        #endregion
    }
}
