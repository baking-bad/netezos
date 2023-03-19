using System.Text.Json;
using System.Text.Json.Serialization;
using Netezos.Encoding.Serialization;
using Netezos.Forging;

namespace Netezos.Encoding
{
    public static class Micheline
    {
        internal const int MaxRecursionDepth = 512;

        static readonly JsonSerializerOptions SerializerOptions = new()
        {
            MaxDepth = 100_000,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public static IMicheline? FromJson(string json)
            => JsonSerializer.Deserialize<IMicheline>(json, SerializerOptions);

        public static IMicheline? FromJson(string json, JsonSerializerOptions options)
            => JsonSerializer.Deserialize<IMicheline>(json, options);

        public static IMicheline? FromJson(JsonElement json)
            => JsonSerializer.Deserialize<IMicheline>(json.GetRawText(), SerializerOptions);

        public static IMicheline? FromJson(JsonElement json, JsonSerializerOptions options)
            => JsonSerializer.Deserialize<IMicheline>(json.GetRawText(), options);

        public static IMicheline FromBytes(byte[] bytes)
        {
            using var mem = new MemoryStream(bytes);
            using var bin = new BinaryReader(mem);
            return MichelineBinaryConverter.Read(bin);
        }

        public static IMicheline Unpack(byte[] bytes)
        {
            if (bytes[0] != 5)
                throw new FormatException("Packed bytes should start with 0x05");

            using var reader = new ForgedReader(bytes.GetBytes(1, bytes.Length - 1));
            return LocalForge.UnforgeMicheline(reader);
        }

        public static string ToMichelson(this IMicheline micheline)
            => MichelsonFormatter.MichelineToMichelson(micheline);

        public static string ToJson(this IMicheline micheline)
            => JsonSerializer.Serialize(micheline, SerializerOptions);

        public static string ToJson(this IMicheline micheline, JsonSerializerOptions options)
            => JsonSerializer.Serialize(micheline, options);

        public static string ToJson(byte[] bytes, JsonWriterOptions options = default)
        {
            using var mem = new MemoryStream(bytes);
            using var reader = new BinaryReader(mem);
            using var res = new MemoryStream();
            using var writer = new Utf8JsonWriter(res, options);

            MichelineBinaryConverter.ReadToJson(reader, writer);
            writer.Flush();

            return Utf8.Convert(res.ToArray());
        }

        public static byte[] ToBytes(this IMicheline micheline)
        {
            using var mem = new MemoryStream();
            using var bin = new BinaryWriter(mem);

            micheline.Write(bin);
            bin.Flush();

            return mem.ToArray();
        }
    }
}
