using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text.Json;

namespace Netezos.Encoding.Serialization
{
    static class MichelineBinaryConverter
    {
        public static IMicheline Read(BinaryReader reader, int depth = 0)
        {
            var tag = reader.ReadByte();
            if (tag >= 0x80)
            {
                var prim = new MichelinePrim { Prim = (PrimType)reader.ReadByte() };

                var args = (tag & 0x70) >> 4;
                if (args > 0)
                {
                    if (args == 0x07)
                        args = reader.Read7BitInt();

                    prim.Args = new List<IMicheline>(args);
                    if (depth < Micheline.MaxRecursionDepth)
                    {
                        while (args-- > 0)
                            prim.Args.Add(Read(reader, depth + 1));
                    }
                    else
                    {
                        while (args-- > 0)
                            prim.Args.Add(ReadFlat(reader));
                    }
                }

                var annots = tag & 0x0F;
                if (annots > 0)
                {
                    if (annots == 0x0F)
                        annots = reader.Read7BitInt();

                    prim.Annots = new List<IAnnotation>(annots);
                    while (annots-- > 0) prim.Annots.Add(ReadAnnotation(reader));
                }

                return prim;
            }
            else
            {
                var cnt = tag & 0x1F;
                if (cnt == 0x1F) cnt = reader.Read7BitInt();

                switch ((MichelineType)(tag & 0xE0))
                {
                    case MichelineType.Array:
                        var arr = new MichelineArray(cnt);
                        if (depth < Micheline.MaxRecursionDepth)
                        {
                            while (cnt-- > 0)
                                arr.Add(Read(reader, depth + 1));
                        }
                        else
                        {
                            while (cnt-- > 0)
                                arr.Add(ReadFlat(reader));
                        }
                        return arr;
                    case MichelineType.Bytes:
                        return new MichelineBytes(reader.ReadBytes(cnt));
                    case MichelineType.Int:
                        return new MichelineInt(new BigInteger(reader.ReadBytes(cnt)));
                    case MichelineType.String:
                        return new MichelineString(Utf8.Convert(reader.ReadBytes(cnt)));
                    default:
                        throw new FormatException("Invalid micheline tag");
                }
            }
        }

        static IAnnotation ReadAnnotation(BinaryReader reader)
        {
            var tag = reader.ReadByte();

            var cnt = tag & 0x3F;
            if (cnt == 0x3F) cnt = reader.Read7BitInt();
            var str = Utf8.Convert(reader.ReadBytes(cnt));

            switch (tag & 0b_1100_0000)
            {
                case (int)AnnotationType.Field:
                    return new FieldAnnotation(str);
                case (int)AnnotationType.Type:
                    return new TypeAnnotation(str);
                case (int)AnnotationType.Variable:
                    return new VariableAnnotation(str);
                default:
                    return new UnsafeAnnotation(str);
            }
        }

        public static void ReadToJson(BinaryReader reader, Utf8JsonWriter writer, int depth = 0)
        {
            var tag = reader.ReadByte();
            if (tag >= 0x80)
            {
                writer.WriteStartObject();
                writer.WriteString("prim", ((PrimType)reader.ReadByte()).ToString());

                var args = (tag & 0x70) >> 4;
                if (args > 0)
                {
                    writer.WritePropertyName("args");
                    writer.WriteStartArray();

                    if (args == 0x07)
                        args = reader.Read7BitInt();

                    if (depth < Micheline.MaxRecursionDepth)
                    {
                        while (args-- > 0)
                            ReadToJson(reader, writer, depth + 1);
                    }
                    else
                    {
                        while (args-- > 0)
                            ReadToJsonFlat(reader, writer);
                    }

                    writer.WriteEndArray();
                }

                var annots = tag & 0x0F;
                if (annots > 0)
                {
                    writer.WritePropertyName("annots");
                    writer.WriteStartArray();

                    if (annots == 0x0F)
                        annots = reader.Read7BitInt();

                    while (annots-- > 0) ReadAnnotationToJson(reader, writer);

                    writer.WriteEndArray();
                }

                writer.WriteEndObject();
            }
            else
            {
                var cnt = tag & 0x1F;
                if (cnt == 0x1F) cnt = reader.Read7BitInt();

                switch ((MichelineType)(tag & 0xE0))
                {
                    case MichelineType.Array:
                        writer.WriteStartArray();
                        if (depth < Micheline.MaxRecursionDepth)
                        {
                            while (cnt-- > 0)
                                ReadToJson(reader, writer, depth + 1);
                        }
                        else
                        {
                            while (cnt-- > 0)
                                ReadToJsonFlat(reader, writer);
                        }
                        writer.WriteEndArray();
                        break;
                    case MichelineType.Bytes:
                        writer.WriteStartObject();
                        writer.WriteString("bytes", Hex.Convert(reader.ReadBytes(cnt)));
                        writer.WriteEndObject();
                        break;
                    case MichelineType.Int:
                        writer.WriteStartObject();
                        writer.WriteString("int", new BigInteger(reader.ReadBytes(cnt)).ToString());
                        writer.WriteEndObject();
                        break;
                    case MichelineType.String:
                        writer.WriteStartObject();
                        writer.WriteString("string", reader.ReadBytes(cnt));
                        writer.WriteEndObject();
                        break;
                    default:
                        throw new FormatException("Invalid micheline tag");
                }
            }
        }

        static void ReadAnnotationToJson(BinaryReader reader, Utf8JsonWriter writer)
        {
            var tag = reader.ReadByte();

            var cnt = tag & 0x3F;
            if (cnt == 0x3F) cnt = reader.Read7BitInt();
            var str = Utf8.Convert(reader.ReadBytes(cnt));

            switch (tag & 0b_1100_0000)
            {
                case (int)AnnotationType.Field:
                    writer.WriteStringValue($"{FieldAnnotation.Prefix}{str}");
                    break;
                case (int)AnnotationType.Type:
                    writer.WriteStringValue($"{TypeAnnotation.Prefix}{str}");
                    break;
                case (int)AnnotationType.Variable:
                    writer.WriteStringValue($"{VariableAnnotation.Prefix}{str}");
                    break;
                default:
                    writer.WriteStringValue(str);
                    break;
            }
        }

        static IMicheline ReadFlat(BinaryReader reader)
        {
            Stack<IMicheline> stack = new();
            IMicheline node, top;
            MichelinePrim prim;
            MichelineArray arr;
            int tag, cnt, args, annots;

        start:
            tag = reader.ReadByte();
            if (tag >= 0x80)
            {
                prim = new() { Prim = (PrimType)reader.ReadByte() };

                annots = tag & 0x0F;
                if (annots > 0)
                {
                    if (annots == 0x0F)
                        annots = reader.Read7BitInt();
                    prim.Annots = new(annots);
                }

                args = (tag & 0x70) >> 4;
                if (args > 0)
                {
                    if (args == 0x07)
                        args = reader.Read7BitInt();
                    prim.Args = new(args);

                    stack.Push(prim);
                    goto start;
                }

                if (prim.Annots != null)
                    ReadAnnots(reader, prim);

                node = prim;
            }
            else
            {
                cnt = tag & 0x1F;
                if (cnt == 0x1F) cnt = reader.Read7BitInt();
                
                switch ((MichelineType)(tag & 0xE0))
                {
                    case MichelineType.Array:
                        node = new MichelineArray(cnt);
                        if (cnt > 0)
                        {
                            stack.Push(node);
                            goto start;
                        }
                        break;
                    case MichelineType.Bytes:
                        node = new MichelineBytes(reader.ReadBytes(cnt));
                        break;
                    case MichelineType.Int:
                        node = new MichelineInt(new BigInteger(reader.ReadBytes(cnt)));
                        break;
                    case MichelineType.String:
                        node = new MichelineString(Utf8.Convert(reader.ReadBytes(cnt)));
                        break;
                    default:
                        throw new FormatException("Invalid micheline tag");
                }
            }
        finish:
            if (stack.Count == 0) return node;
            top = stack.Peek();
            if (top is MichelinePrim p)
            {
                p.Args.Add(node);
                if (p.Args.Count < p.Args.Capacity)
                    goto start;
                if (p.Annots != null)
                    ReadAnnots(reader, p);
            }
            else
            {
                arr = (MichelineArray)top;
                arr.Add(node);
                if (arr.Count < arr.Capacity)
                    goto start;
            }
            node = stack.Pop();
            goto finish;
        }

        static void ReadAnnots(BinaryReader reader, MichelinePrim prim)
        {
            while (prim.Annots.Count != prim.Annots.Capacity)
            {
                var tag = reader.ReadByte();

                var cnt = tag & 0x3F;
                if (cnt == 0x3F) cnt = reader.Read7BitInt();
                var str = Utf8.Convert(reader.ReadBytes(cnt));

                switch (tag & 0b_1100_0000)
                {
                    case (int)AnnotationType.Field:
                        prim.Annots.Add(new FieldAnnotation(str));
                        break;
                    case (int)AnnotationType.Type:
                        prim.Annots.Add(new TypeAnnotation(str));
                        break;
                    case (int)AnnotationType.Variable:
                        prim.Annots.Add(new VariableAnnotation(str));
                        break;
                    default:
                        prim.Annots.Add(new UnsafeAnnotation(str));
                        break;
                }
            }
        }

        static void ReadToJsonFlat(BinaryReader reader, Utf8JsonWriter writer)
        {
            Stack<(int, int, int)> stack = new();
            int tag, cnt, args, annots;

        start:
            tag = reader.ReadByte();
            if (tag >= 0x80)
            {
                writer.WriteStartObject();
                writer.WriteString("prim", ((PrimType)reader.ReadByte()).ToString());

                annots = tag & 0x0F;
                if (annots > 0)
                {
                    if (annots == 0x0F)
                        annots = reader.Read7BitInt();
                }

                args = (tag & 0x70) >> 4;
                if (args > 0)
                {
                    if (args == 0x07)
                        args = reader.Read7BitInt();

                    writer.WriteStartArray("args");
                    stack.Push((0, args, annots));
                    goto start;
                }

                if (annots > 0)
                    ReadAnnotsToJson(reader, writer, annots);
                
                writer.WriteEndObject();
            }
            else
            {
                cnt = tag & 0x1F;
                if (cnt == 0x1F) cnt = reader.Read7BitInt();

                switch ((MichelineType)(tag & 0xE0))
                {
                    case MichelineType.Array:
                        writer.WriteStartArray();
                        if (cnt > 0)
                        {
                            stack.Push((1, cnt, 0));
                            goto start;
                        }
                        writer.WriteEndArray();
                        break;
                    case MichelineType.Bytes:
                        writer.WriteStartObject();
                        writer.WriteString("bytes", Hex.Convert(reader.ReadBytes(cnt)));
                        writer.WriteEndObject();
                        break;
                    case MichelineType.Int:
                        writer.WriteStartObject();
                        writer.WriteString("int", new BigInteger(reader.ReadBytes(cnt)).ToString());
                        writer.WriteEndObject();
                        break;
                    case MichelineType.String:
                        writer.WriteStartObject();
                        writer.WriteString("string", reader.ReadBytes(cnt));
                        writer.WriteEndObject();
                        break;
                    default:
                        throw new FormatException("Invalid micheline tag");
                }
            }
        finish:
            if (stack.Count == 0) return;
            var (type, cnt1, cnt2) = stack.Pop();
            if (cnt1 > 1)
            {
                stack.Push((type, cnt1 - 1, cnt2));
                goto start;
            }
            if (type == 0)
            {
                writer.WriteEndArray();
                if (cnt2 > 0) ReadAnnotsToJson(reader, writer, cnt2);
                writer.WriteEndObject();
            }
            else
            {
                writer.WriteEndArray();
            }
            goto finish;
        }

        static void ReadAnnotsToJson(BinaryReader reader, Utf8JsonWriter writer, int count)
        {
            writer.WriteStartArray("annots");
            while (count-- > 0)
            {
                var tag = reader.ReadByte();

                var cnt = tag & 0x3F;
                if (cnt == 0x3F) cnt = reader.Read7BitInt();
                var str = Utf8.Convert(reader.ReadBytes(cnt));

                switch (tag & 0b_1100_0000)
                {
                    case (int)AnnotationType.Field:
                        writer.WriteStringValue($"{FieldAnnotation.Prefix}{str}");
                        break;
                    case (int)AnnotationType.Type:
                        writer.WriteStringValue($"{TypeAnnotation.Prefix}{str}");
                        break;
                    case (int)AnnotationType.Variable:
                        writer.WriteStringValue($"{VariableAnnotation.Prefix}{str}");
                        break;
                    default:
                        writer.WriteStringValue(str);
                        break;
                }
            }
            writer.WriteEndArray();
        }

        public static void WriteFlat(BinaryWriter writer, IMicheline value)
        {
            var stack = new Stack<object>();
            stack.Push(value);

            int i, len, tag;
            bool argsCount;
            byte[] bytes;
            while (stack.Count > 0)
            {
                switch (stack.Pop())
                {
                    case MichelineArray array:
                        if (array.Count >= 0x1F)
                        {
                            writer.Write((byte)((int)array.Type | 0x1F));
                            writer.Write7BitInt(array.Count);
                        }
                        else
                        {
                            writer.Write((byte)((int)array.Type | array.Count));
                        }
                        for (i = array.Count - 1; i >= 0; i--)
                            stack.Push(array[i]);
                        break;
                    case MichelinePrim prim:
                        argsCount = false;
                        tag = (int)prim.Type;
                        if (prim.Args?.Count > 0)
                        {
                            if (prim.Args.Count >= 0x07)
                            {
                                tag |= 0x70;
                                argsCount = true;
                            }
                            else
                            {
                                tag |= prim.Args.Count << 4;
                            }
                        }
                        if (prim.Annots?.Count > 0)
                        {
                            stack.Push(prim.Annots);
                            if (prim.Annots.Count >= 0x0F)
                            {
                                tag |= 0x0F;
                                stack.Push(prim.Annots.Count);
                            }
                            else
                            {
                                tag |= prim.Annots.Count;
                            }
                        }
                        writer.Write((byte)tag);
                        writer.Write((byte)prim.Prim);
                        if (prim.Args != null)
                        {
                            if (argsCount)
                                writer.Write7BitInt(prim.Args.Count);

                            for (i = prim.Args.Count - 1; i >= 0; i--)
                                stack.Push(prim.Args[i]);
                        }
                        break;
                    case MichelineBytes micheBytes:
                        bytes = micheBytes.Value;
                        len = micheBytes.Value.Length;
                        if (len >= 0x1F)
                        {
                            writer.Write((byte)((int)micheBytes.Type | 0x1F));
                            writer.Write7BitInt(len);
                        }
                        else
                        {
                            writer.Write((byte)((int)micheBytes.Type | len));
                        }
                        writer.Write(bytes);
                        break;
                    case MichelineString micheString:
                        bytes = Utf8.Parse(micheString.Value);
                        len = bytes.Length;
                        if (len >= 0x1F)
                        {
                            writer.Write((byte)((int)micheString.Type | 0x1F));
                            writer.Write7BitInt(len);
                        }
                        else
                        {
                            writer.Write((byte)((int)micheString.Type | len));
                        }
                        writer.Write(bytes);
                        break;
                    case MichelineInt micheInt:
                        bytes = micheInt.Value.ToByteArray();
                        len = bytes.Length;
                        if (len >= 0x1F)
                        {
                            writer.Write((byte)((int)micheInt.Type | 0x1F));
                            writer.Write7BitInt(len);
                        }
                        else
                        {
                            writer.Write((byte)((int)micheInt.Type | len));
                        }
                        writer.Write(bytes);
                        break;
                    case int intValue:
                        writer.Write7BitInt(intValue);
                        break;
                    case IEnumerable<IAnnotation> annots:
                        foreach (var annot in annots)
                        {
                            bytes = Utf8.Parse(annot.Value);
                            len = bytes.Length;
                            if (len >= 0x3F)
                            {
                                writer.Write((byte)((int)annot.Type | 0x3F));
                                writer.Write7BitInt(len);
                            }
                            else
                            {
                                writer.Write((byte)((int)annot.Type | len));
                            }
                            writer.Write(bytes);
                        }
                        break;
                    default:
                        throw new Exception();
                }
            }
        }
    }
}
