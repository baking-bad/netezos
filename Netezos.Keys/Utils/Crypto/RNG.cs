using System;
using System.Security.Cryptography;

namespace Netezos.Keys.Utils.Crypto
{
    public static class RNG
    {
        private static readonly RandomNumberGenerator Rng = RandomNumberGenerator.Create();

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
    }
}