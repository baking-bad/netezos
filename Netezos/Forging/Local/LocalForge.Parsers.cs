using Netezos.Encoding;
using System;
using System.Linq;
using System.Numerics;

namespace Netezos.Forging
{
    public partial class LocalForge : IForge
    {
        static bool ParseBool(byte[] bytes)
        {
            return bytes[0] == 255;
        }

        static int ParseInt32(byte[] bytes)
        {
            int res = 0;
            for (int i = 0, shift = 24; i < 4; i++, shift -= 8)
                res |= bytes[i] << shift;
            return res;
        }

        static long ParseInt64(byte[] bytes)
        {
            var idx = 0;

            var i1 = 0;
            for (int i = 0, shift = 24; i < 4; i++, shift -= 8, idx++)
                i1 |= bytes[idx] << shift;

            var i2 = 0;
            for (int i = 0, shift = 24; i < 4; i++, shift -= 8, idx++)
                i2 |= bytes[idx] << shift;

            return (uint)i2 | ((long)i1 << 32);
        }

        static (byte[], int) ParseArray(byte[] bytes, int len = 4)
        {
            int arrLen = ParseInt32(bytes);
            return (bytes.GetBytes(len, arrLen), len + arrLen);
        }
        static (string, int) ParseString(byte[] bytes)
        {
            int len = ParseInt32(bytes);
            return (Utf8.Convert(bytes.GetBytes(4, len)), len);
        }

        static string ParsePublicKey(byte[] bytes)
        {
            byte[] prefix;

            switch (bytes[0])
            {
                case 0: prefix = Prefix.edpk; break;
                case 1: prefix = Prefix.sppk; break;
                case 2: prefix = Prefix.p2pk; break;
                default: throw new ArgumentException($"Invalid public key prefix {bytes[0]}");
            };

            return Base58.Convert(bytes.GetBytes(1, bytes.Length - 1), prefix);
        }

        static string ParseAddress(byte[] bytes)
        {
            byte[] prefix = null;
            int len = 0;
            int offset = 0;

            if (bytes[0] == 0)
            {
                offset = 2;
                len = bytes.Length - offset;

                if (bytes[1] == 0)
                {
                    prefix = Prefix.tz1;
                }
                else if (bytes[1] == 1)
                {
                    prefix = Prefix.tz2;
                }
                else if (bytes[2] == 2)
                {
                    prefix = Prefix.tz3;
                }
            }
            else if (bytes[0] == 1)
            {
                offset = 1;
                len = bytes.Length - offset - 1;
                prefix = Prefix.KT1;
            }

            if (prefix == null)
            {
                throw new ArgumentException($"Invalid address prefix {bytes[0]}");
            }

            string address = Base58.Convert(bytes.GetBytes(offset, len), prefix);

            return address;
        }

        static string ParseTzAddress(byte[] bytes)
        {
            byte[] prefix;

            switch (bytes[0])
            {
                case 0: prefix = Prefix.tz1; break;
                case 1: prefix = Prefix.tz2; break;
                case 2: prefix = Prefix.tz3; break;
                default:
                    throw new ArgumentException($"Invalid source prefix {bytes[0]}");
            }

            return Base58.Convert(bytes.GetBytes(1, bytes.Length - 1), prefix);
        }

        static string ParseTz1Address(byte[] bytes)
        {
            return Base58.Convert(bytes.GetBytes(1, bytes.Length - 1), Prefix.tz1);
        }

        static (int, int) ParseMicheNat(byte[] bytes)
        {
            int value = 0;
            int len = 1;

            while ((bytes[len - 1] & 0x80) != 0)
            {
                len++;
            }

            for (int i = len - 1; i >= 0; i--)
            {
                value <<= 7;
                value |= (bytes[i] & 0x7F);
            }

            return (value, len);
        }

        static (BigInteger, int) ParseMicheInt(byte[] data)
        {
            int value = 0;
            int len = 1;

            while ((data[len - 1] & 0x80) != 0)
            {
                len++;
            }

            for (int i = len - 1; i > 0; i--)
            {
                value <<= 7;
                value |= (data[i] & 0x7F);
            }

            value <<= 6;
            value |= data[0] & 0x3F;

            if ((data[0] & 0x40) != 0)
            {
                value = -value;
            }

            return (value, len);
        }

        static string ParseEntrypoint(byte[] bytes)
        {
            switch (bytes[0])
            {
                case 0: return "default";
                case 1: return "root";
                case 2: return "do";
                case 3: return "set_delegate";
                case 4: return "remove_delegate";
                case 255: return ParseString(bytes.GetBytes(1, bytes.Length - 1)).Item1;
                default:
                    throw new ArgumentException($"Invalid entrypoint prefix {bytes[0]}");
            }
        }


        static IMicheline UnforgeMicheline(byte[] data)
        {
            int ptr = 0;

            IMicheline Parse()
            {
                byte tag = data[ptr];

                ptr++;

                switch (tag)
                {
                    case 0:
                        (BigInteger bigInt, int intLen) = ParseMicheInt(data.GetBytes(ptr));
                        ptr += intLen;
                        return new MichelineInt(bigInt);

                    case 1:
                        (string s, int sLen) = ParseString(data.GetBytes(ptr));
                        ptr += sLen + 4;
                        return new MichelineString(s);

                    case 2: return ParseList();

                    case 3: return ParsePrim(0, false);

                    case 4: return ParsePrim(0, true);

                    case 5: return ParsePrim(1, false);

                    case 6: return ParsePrim(1, true);

                    case 7: return ParsePrim(2, false);

                    case 8: return ParsePrim(2, true);

                    case 9: return ParsePrim(3, true);

                    case 10:
                        (byte[] bs, int arrLen) = ParseArray(data.GetBytes(ptr));

                        ptr += arrLen;

                        return new MichelineBytes(bs);

                    default:
                        throw new InvalidOperationException($"Unknown tag {tag} at position {ptr}");
                }
            }

            MichelineArray ParseList()
            {
                (byte[] arr, int offset) = ParseArray(data.GetBytes(ptr));

                int end = ptr + offset;

                MichelineArray res = new MichelineArray();

                ptr += 4;

                while (ptr < end)
                {
                    res.Add(Parse());
                }

                return res;
            }

            MichelinePrim ParsePrim(int argsLength, bool annotations = false)
            {
                PrimType primTag = (PrimType)data[ptr];

                ptr++;

                MichelinePrim prim = new MichelinePrim
                {
                    Prim = primTag
                };

                if (0 < argsLength && argsLength < 3)
                {
                    for (int i = 0; i < argsLength; i++)
                    {
                        prim.Args.Add(Parse());
                    }
                }
                else if (argsLength == 3)
                {
                    prim.Args = ParseList();
                }
                else
                {
                    throw new ArgumentException($"Unexpected args length {argsLength}", nameof(argsLength));
                }

                if (annotations)
                {
                    (byte[] value, int offset) = ParseArray(data.GetBytes(ptr));

                    ptr += offset;

                    if (value.Length > 0)
                    {
                        prim.Annots = ParseString(value)
                            .Item1
                            .Split(' ')
                            .Select(a =>
                            {
                                string annotVal = a.Substring(1);

                                IAnnotation annotation;

                                switch (a[0])
                                {
                                    case FieldAnnotation.Prefix:
                                        annotation = new FieldAnnotation(annotVal);
                                        break;

                                    case TypeAnnotation.Prefix:
                                        annotation = new TypeAnnotation(annotVal);
                                        break;

                                    case VariableAnnotation.Prefix:
                                        annotation = new VariableAnnotation(annotVal);
                                        break;

                                    default:
                                        throw new InvalidOperationException($"Unknown annotation type: {a[0]}");
                                }

                                return annotation;
                            })
                            .ToList();
                    }
                }

                return prim;
            }

            IMicheline result = Parse();

            if (ptr != data.Length)
            {
                throw new ArgumentException($"Did not reach EOS (pos {ptr}/{data.Length})");
            }

            return result;
        }
    }
}
