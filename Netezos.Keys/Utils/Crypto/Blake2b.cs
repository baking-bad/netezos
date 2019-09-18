using Org.BouncyCastle.Crypto.Digests;

namespace Netezos.Keys.Utils.Crypto
{
    static class Blake2b
    {
        public static byte[] GetDigest(byte[] msg, int size)
        {
            var digest = new Blake2bDigest(size);
            foreach (var t in msg)
            {
                digest.Update(t);
            }
            var keyedHash = new byte[size/8];
            digest.DoFinal(keyedHash, 0);
            return keyedHash;
        }

        public static byte[] GetDigest(byte[] msg) => GetDigest(msg, 256);
    }
}