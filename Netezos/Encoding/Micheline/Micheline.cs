using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Netezos.Encoding.Serialization;

namespace Netezos.Encoding
{
    public static class Micheline
    {
        static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
        {
            MaxDepth = 1024,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public static IMicheline FromJson(string json)
            => JsonSerializer.Deserialize<IMicheline>(json, SerializerOptions);

        public static IMicheline FromJson(string json, JsonSerializerOptions options)
            => JsonSerializer.Deserialize<IMicheline>(json, options);

        public static IMicheline FromJson(JsonElement json)
            => JsonSerializer.Deserialize<IMicheline>(json.GetRawText(), SerializerOptions);

        public static IMicheline FromJson(JsonElement json, JsonSerializerOptions options)
            => JsonSerializer.Deserialize<IMicheline>(json.GetRawText(), options);

        public static IMicheline FromBytes(byte[] bytes)
        {
            using (var mem = new MemoryStream(bytes))
            using (var bin = new BinaryReader(mem))
            {
                return Read(bin);
            }
        }

        public static string ToMichelson(this IMicheline micheline)
            => MichelsonFormatter.MichelineToMichelson(micheline);

        public static string ToJson(this IMicheline micheline)
            => JsonSerializer.Serialize(micheline, SerializerOptions);

        public static string ToJson(this IMicheline micheline, JsonSerializerOptions options)
            => JsonSerializer.Serialize(micheline, options);

        public static string ToJson(byte[] bytes)
        {
            using (var mem = new MemoryStream(bytes))
            using (var reader = new BinaryReader(mem))
            using (var res = new MemoryStream())
            using (var writer = new Utf8JsonWriter(res))
            {
                ReadToJson(reader, writer);
                writer.Flush();
                return Utf8.Convert(res.ToArray());
            }
        }

        public static byte[] ToBytes(this IMicheline micheline)
        {
            using (var mem = new MemoryStream())
            using (var bin = new BinaryWriter(mem))
            {
                micheline.Write(bin);
                return mem.ToArray();
            }
        }

        static IMicheline Read(BinaryReader reader)
        {
            var tag = reader.ReadByte();
            if (tag >= 0x80)
            {
                var prim = new MichelinePrim();
                prim.Prim = (PrimType)reader.ReadByte();

                var args = (tag & 0x70) >> 4;
                if (args > 0)
                {
                    if (args == 0x07)
                        args = reader.Read7BitInt();

                    prim.Args = new List<IMicheline>(args);
                    while (args-- > 0) prim.Args.Add(Read(reader));
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
                        while (cnt-- > 0) arr.Add(Read(reader));
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
                    throw new FormatException("Invalid annotation tag");
            }
        }

        static void ReadToJson(BinaryReader reader, Utf8JsonWriter writer)
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

                    while (args-- > 0) ReadToJson(reader, writer);

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
                        while (cnt-- > 0) ReadToJson(reader, writer);
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
                    writer.WriteStringValue($"%{str}");
                    break;
                case (int)AnnotationType.Type:
                    writer.WriteStringValue($":{str}");
                    break;
                case (int)AnnotationType.Variable:
                    writer.WriteStringValue($"@{str}");
                    break;
                default:
                    throw new FormatException("Invalid annotation tag");
            }
        }
    }
}
