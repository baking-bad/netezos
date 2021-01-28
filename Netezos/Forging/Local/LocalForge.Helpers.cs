using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Netezos.Encoding;

namespace Netezos.Forging
{
    public partial class LocalForge : IForge
    {
        static byte[] Concat(params byte[][] arrays)
        {
            var res = new byte[arrays.Sum(x => x.Length)];

            for (int s = 0, i = 0; i < arrays.Length; s += arrays[i].Length, i++)
                Buffer.BlockCopy(arrays[i], 0, res, s, arrays[i].Length);

            return res;
        }

        static byte[] ForgeBool(bool value)
        {
            return value ? new byte[] { 255 } : new byte[] { 0 };
        }

        static bool UnforgeBool(byte[] bytes)
        {
            return bytes[0] == 255;
        }

        static byte[] ForgeInt32(int value, int len = 4)
        {
            var res = new byte[len];
            for (int i = len - 1; i >= 0; i--, value >>= 8)
                res[i] = (byte)(value & 0xFF);
            return res;
        }

        static int UnforgeInt32(byte[] bytes)
        {
            int res = 0;
            for (int i = 0, shift = 24; i < 4; i++, shift -= 8)
                res |= bytes[i] << shift;
            return res;
        }

        static byte[] ForgeInt64(long value, int len = 8)
        {
            var res = new byte[len];
            for (int i = len - 1; i >= 0; i--, value >>= 8)
                res[i] = (byte)(value & 0xFF);
            return res;
        }

        static long UnforgeInt64(byte[] bytes)
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

        static byte[] ForgeArray(byte[] value, int len = 4)
        {
            return ForgeInt32(value.Length, len).Concat(value);
        }

        static byte[] ForgeString(string value, int len = 4)
        {
            return ForgeInt32(value.Length, len).Concat(Utf8.Parse(value));
        }

        static string UnforgeString(byte[] bytes)
        {
            return Utf8.Convert(bytes.GetBytes(4, UnforgeInt32(bytes)));
        }

        static byte[] ForgePublicKey(string value)
        {
            var prefix = value.Substring(0, 4);
            var res = Base58.Parse(value, 4);

            switch (prefix)
            {
                case "edpk": return new byte[] { 0 }.Concat(res);
                case "sppk": return new byte[] { 1 }.Concat(res);
                case "p2pk": return new byte[] { 2 }.Concat(res);
                default:
                    throw new ArgumentException($"Invalid public key prefix {prefix}");
            }
        }

        static string UnforgePublicKey(byte[] bytes)
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

        static byte[] ForgeAddress(string value)
        {
            var prefix = value.Substring(0, 3);
            var res = Base58.Parse(value, 3);

            switch (prefix)
            {
                case "tz1": return new byte[] { 0, 0 }.Concat(res);
                case "tz2": return new byte[] { 0, 1 }.Concat(res);
                case "tz3": return new byte[] { 0, 2 }.Concat(res);
                case "KT1": return new byte[] { 1 }.Concat(res).Concat(new byte[] { 0 });
                default:
                    throw new ArgumentException($"Invalid address prefix {prefix}");
            }
        }

        static string UnforgeAddress(byte[] bytes)
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

        static byte[] ForgeTzAddress(string value)
        {
            var prefix = value.Substring(0, 3);
            var res = Base58.Parse(value, 3);

            switch (prefix)
            {
                case "tz1": return new byte[] { 0 }.Concat(res);
                case "tz2": return new byte[] { 1 }.Concat(res);
                case "tz3": return new byte[] { 2 }.Concat(res);
                default:
                    throw new ArgumentException($"Invalid source prefix {prefix}");
            }
        }

        static string UnforgeTzAddress(byte[] bytes)
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

        static byte[] ForgeTz1Address(string value)
        {
            return Base58.Parse(value, 3);
        }

        static string UnforgeTz1Address(byte[] bytes)
        {
            return Base58.Convert(bytes.GetBytes(1, bytes.Length - 1), Prefix.tz1);
        }

        static byte[] ForgeMicheNat(int value)
        {
            if (value < 0)
                throw new ArgumentException("Nat cannot be negative");

            var res = new List<byte>(9);
            res.Add((byte)(value & 0x7F));
            value >>= 7;

            while (value > 0)
            {
                res[res.Count - 1] |= 0x80;
                res.Add((byte)(value & 0x7F));
                value >>= 7;
            }

            return res.ToArray();
        }

        static byte[] ForgeMicheNat(long value)
        {
            if (value < 0)
                throw new ArgumentException("Nat cannot be negative");

            var res = new List<byte>(9);
            res.Add((byte)(value & 0x7F));
            value >>= 7;

            while (value > 0)
            {
                res[res.Count - 1] |= 0x80;
                res.Add((byte)(value & 0x7F));
                value >>= 7;
            }

            return res.ToArray();
        }

        static byte[] ForgeMicheInt(BigInteger value)
        {
            var abs = BigInteger.Abs(value);
            var res = new List<byte>();

            res.Add((byte)(value.Sign < 0 ? (abs & 0x3F | 0x40) : (abs & 0x3F)));
            abs >>= 6;

            while (abs > 0)
            {
                res[res.Count - 1] |= 0x80;
                res.Add((byte)(abs & 0x7F));
                abs >>= 7;
            }

            return res.ToArray();
        }

        static byte[] ForgeEntrypoint(string value)
        {
            switch (value)
            {
                case "default": return new byte[] { 0 };
                case "root": return new byte[] { 1 };
                case "do": return new byte[] { 2 };
                case "set_delegate": return new byte[] { 3 };
                case "remove_delegate": return new byte[] { 4 };
                default:
                    return new byte[] { 255 }.Concat(ForgeString(value, 1));
            }
        }

        static string UnforgeEntrypoint(byte[] bytes)
        {
            switch (bytes[0])
            {
                case 0: return "default";
                case 1: return "root";
                case 2: return "do";
                case 3: return "set_delegate";
                case 4: return "remove_delegate";
                case 255: return UnforgeString(bytes.GetBytes(1, bytes.Length - 1));
                default:
                    throw new ArgumentException($"Invalid entrypoint prefix {bytes[0]}");
            }
        }

        static byte[] ForgeMicheline(IMicheline micheline)
        {
            switch (micheline)
            {
                case MichelinePrim prim:
                    var res = new List<byte>();

                    var argsCnt = prim.Args?.Count ?? 0;
                    var annots = prim.Annots == null ? 0 : 1;
                    var tag = Math.Min(argsCnt * 2 + 3 + annots, 9);

                    res.Add((byte)tag);
                    res.Add((byte)prim.Prim);

                    if (argsCnt > 0)
                    {
                        var args = prim.Args.Select(ForgeMicheline).SelectMany(x => x);
                        res.AddRange(argsCnt < 3 ? args : ForgeArray(args.ToArray()));
                    }

                    if (annots > 0)
                    {
                        res.AddRange(ForgeString(string.Join(" ", prim.Annots)));
                    }
                    else if (argsCnt >= 3)
                    {
                        res.AddRange(new List<byte> { 0, 0, 0, 0 });
                    }

                    return res.ToArray();

                case MichelineArray array:
                    return new byte[] { 2 }.Concat(ForgeArray(array.Select(ForgeMicheline).SelectMany(x => x).ToArray()));

                case MichelineInt micheInt:
                    return new byte[] { 0 }.Concat(ForgeMicheInt(micheInt.Value));

                case MichelineString micheString:
                    return new byte[] { 1 }.Concat(ForgeString(micheString.Value));

                case MichelineBytes micheBytes:
                    return new byte[] { 10 }.Concat(ForgeArray(micheBytes.Value));

                default:
                    throw new ArgumentException("Invalid micheline node type");
            }
        }

        //static IMicheline UnforgeMicheline(byte[] bytes)
        //{
        //    switch (micheline)
        //    {
        //        case MichelinePrim prim:
        //            var res = new List<byte>();

        //            var argsCnt = prim.Args?.Count ?? 0;
        //            var annots = prim.Annots == null ? 0 : 1;
        //            var tag = Math.Min(argsCnt * 2 + 3 + annots, 9);

        //            res.Add((byte)tag);
        //            res.Add((byte)prim.Prim);

        //            if (argsCnt > 0)
        //            {
        //                var args = prim.Args.Select(ForgeMicheline).SelectMany(x => x);
        //                res.AddRange(argsCnt < 3 ? args : ForgeArray(args.ToArray()));
        //            }

        //            if (annots > 0)
        //            {
        //                res.AddRange(ForgeString(string.Join(" ", prim.Annots)));
        //            }
        //            else if (argsCnt >= 3)
        //            {
        //                res.AddRange(new List<byte> { 0, 0, 0, 0 });
        //            }
                    
        //            return res.ToArray();

        //        case MichelineArray array:
        //            return new byte[] { 2 }.Concat(ForgeArray(array.Select(ForgeMicheline).SelectMany(x => x).ToArray()));

        //        case MichelineInt micheInt:
        //            return new byte[] { 0 }.Concat(ForgeMicheInt(micheInt.Value));

        //        case MichelineString micheString:
        //            return new byte[] { 1 }.Concat(ForgeString(micheString.Value));

        //        case MichelineBytes micheBytes:
        //            return new byte[] { 10 }.Concat(ForgeArray(micheBytes.Value));

        //        default:
        //            throw new ArgumentException("Invalid micheline node type");
        //    }
        //}
    }
}
