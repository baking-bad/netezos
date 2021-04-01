using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Netezos.Encoding
{
    class HexConverter : JsonConverter<byte[]>
    {
        public override byte[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return Hex.Parse(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, byte[] value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(Hex.Convert(value));
        }
    }
}
