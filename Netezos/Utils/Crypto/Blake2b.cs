using Org.BouncyCastle.Crypto.Digests;

namespace Netezos.Utils
{
    public static class Blake2b
    {
        public static byte[] GetDigest(byte[] msg) => GetDigest(msg, 256);

        public static byte[] GetDigest(byte[] msg, int size)
        {
            var result = new byte[size/8];
            var digest = new Blake2bDigest(size);

            digest.BlockUpdate(msg, 0, msg.Length);
            digest.DoFinal(result, 0);

            return result;
        }
    }
}