using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    class Int64NullableStringConverter : JsonConverter<long?>
    {
        public override long? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType == JsonTokenType.String
                ? long.Parse(reader.GetString())
                : reader.TokenType == JsonTokenType.Number
                    ? reader.GetInt64()
                    : null;
        }

        public override void Write(Utf8JsonWriter writer, long? value, JsonSerializerOptions options)
        {
            if (value != null)
                writer.WriteStringValue(value.ToString());
            else
                writer.WriteNullValue();
        }
    }
}
