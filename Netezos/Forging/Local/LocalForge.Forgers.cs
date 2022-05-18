﻿using Netezos.Encoding;
using Netezos.Forging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Netezos.Forging
{
    public partial class LocalForge
    {
        public static byte[] ForgeTag(OperationTag tag)
        {
            return new[] { (byte)tag };
        }

        public static byte[] ForgeBool(bool value)
        {
            return value ? new byte[] { 255 } : new byte[] { 0 };
        }

        public static byte[] ForgeInt32(int value, int len = 4)
        {
            var res = new byte[len];
            for (int i = len - 1; i >= 0; i--, value >>= 8)
                res[i] = (byte)(value & 0xFF);
            return res;
        }

        public static byte[] ForgeInt64(long value, int len = 8)
        {
            var res = new byte[len];
            for (int i = len - 1; i >= 0; i--, value >>= 8)
                res[i] = (byte)(value & 0xFF);
            return res;
        }

        public static byte[] ForgeArray(byte[] value, int len = 4)
        {
            return ForgeInt32(value.Length, len).Concat(value);
        }

        public static byte[] ForgeString(string value, int len = 4)
        {
            return ForgeInt32(value.Length, len).Concat(Utf8.Parse(value));
        }

        public static byte[] ForgePublicKey(string value)
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

        public static byte[] ForgeAddress(string value)
        {
            var prefix = value.Substring(0, 3);
            var res = Base58.Parse(value, 3);

            switch (prefix)
            {
                case "tz1": return new byte[] { 0, 0 }.Concat(res);
                case "tz2": return new byte[] { 0, 1 }.Concat(res);
                case "tz3": return new byte[] { 0, 2 }.Concat(res);
                case "KT1": return new byte[] { 1 }.Concat(res).Concat(new byte[] { 0 });
                case "txr1": return new byte[] { 2 }.Concat(res).Concat(new byte[] { 0 });
                default:
                    throw new ArgumentException($"Invalid address prefix {prefix}");
            }
        }

        public static byte[] ForgeTzAddress(string value)
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

        public static byte[] ForgePkh(string value)
        {
            return Base58.Parse(value, 3);
        }

        public static byte[] ForgeMicheNat(int value)
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

        public static byte[] ForgeMicheNat(long value)
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

        public static byte[] ForgeMicheNat(BigInteger value)
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

        public static byte[] ForgeMicheInt(BigInteger value)
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

        public static byte[] ForgeEntrypoint(string value)
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

        public static byte[] ForgeMicheline(IMicheline micheline)
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
    }
}
