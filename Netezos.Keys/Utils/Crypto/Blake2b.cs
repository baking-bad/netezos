using Org.BouncyCastle.Crypto.Digests;

namespace Netezos.Keys.Utils.Crypto
{
    static class Blake2b
    {
        public static byte[] GetDigest(byte[] msg) {
            var digest = new Blake2bDigest(256);
            foreach (var t in msg)
            {
                digest.Update(t);
            }
            var keyedHash = new byte[32];
            digest.DoFinal(keyedHash, 0);
            return keyedHash;
        }
    }
}