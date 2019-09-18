using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;

namespace Netezos.Keys
{
    public static class Base58
    {
        const string Digits = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";

        static string Encode(byte[] data)
        {
            return EncodePlain(_AddCheckSum(data));
        }

        static string EncodePlain(byte[] data)
        {
            BigInteger bigInteger = data.Aggregate(0, (Func<BigInteger, byte, BigInteger>) ((current, t) => current * (BigInteger) 256 + (BigInteger) t));

            string str = string.Empty;
            while (bigInteger > 0L)
            {
                int index = (int) (bigInteger % 58);
                bigInteger /= 58;
                str = Digits[index] + str;
            }
            for (int index = 0; index < data.Length && data[index] == (byte) 0; ++index)
                str = "1" + str;
            return str;
        }

        static byte[] Decode(string data)
        {
            byte[] numArray = _VerifyAndRemoveCheckSum(DecodePlain(data));
            if (numArray == null)
                throw new FormatException("Base58 checksum is invalid");
            return numArray;
        }

        static byte[] DecodePlain(string data)
        {
            BigInteger bigInteger = 0;
            for (int index = 0; index < data.Length; ++index)
            {
                int num = Digits.IndexOf(data[index]);
                if (num < 0)
                    throw new FormatException($"Invalid Base58 character `{(object) data[index]}` at position {(object) index}");
                bigInteger = bigInteger * 58 + num;
            }
            return Enumerable.Repeat((byte) 0, data.TakeWhile(c => c == '1').Count()).Concat(bigInteger.ToByteArray().Reverse().SkipWhile(b => b == (byte) 0)).ToArray();
        }

        static byte[] _AddCheckSum(byte[] data)
        {
            byte[] checkSum = _GetCheckSum(data);
            return data.Concat(checkSum);
        }

        static byte[] _VerifyAndRemoveCheckSum(byte[] data)
        {
            byte[] data1 = data.GetBytes(0, data.Length - 4);
            if (!data.GetBytes(data.Length - 4, 4).SequenceEqual(_GetCheckSum(data1)))
                return null;
            return data1;
        }

        static byte[] _GetCheckSum(byte[] data)
        {
            SHA256 shA256 = (SHA256) new SHA256Managed();
            byte[] hash1 = shA256.ComputeHash(data);
            byte[] hash2 = shA256.ComputeHash(hash1);
            byte[] numArray = new byte[4];
            Buffer.BlockCopy((Array) hash2, 0, numArray, 0, numArray.Length);
            return numArray;
        }
        
        public static byte[] Decode(string encoded, byte[] prefix)
        {
            int prefixLen = prefix?.Length ?? 0;

            byte[] msg = Decode(encoded);

            byte[] result = new byte[msg.Length - prefixLen];

            Array.Copy(msg, prefixLen, result, 0, result.Length);

            return result;
        }
        
        public static string Encode(byte[] payload, byte[] prefix)
        {
            int prefixLen = prefix?.Length ?? 0;

            byte[] msg = new byte[prefixLen + payload.Length];

            if (prefix != null)
            {
                Array.Copy(prefix, 0, msg, 0, prefix.Length);
            }

            Array.Copy(payload, 0, msg, prefixLen, payload.Length);

            return Encode(msg);
        }
        
    }
}