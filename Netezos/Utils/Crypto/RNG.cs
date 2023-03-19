using System.Security.Cryptography;

namespace Netezos.Utils
{
    public static class RNG
    {
        static readonly RandomNumberGenerator Rng = RandomNumberGenerator.Create();

        public static void WriteBytes(byte[] dst)
        {
            Rng.GetBytes(dst);
        }

        public static void WriteNonZeroBytes(byte[] dst)
        {
            Rng.GetNonZeroBytes(dst);
        }

        public static byte[] GetBytes(int length)
        {
            var buf = new byte[length];
            Rng.GetBytes(buf);
            return buf;
        }

        public static byte[] GetNonZeroBytes(int length)
        {
            var buf = new byte[length];
            Rng.GetNonZeroBytes(buf);
            return buf;
        }

        public static int GetInt32()
        {
            return BitConverter.ToInt32(GetBytes(4), 0);
        }

        public static long GetInt64()
        {
            return BitConverter.ToInt64(GetBytes(8), 0);
        }
    }
}