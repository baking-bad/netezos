using System;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    class BigIntegerNullableStringConverter : JsonConverter<BigInteger?>
    {
        public override BigInteger? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType == JsonTokenType.String
                ? BigInteger.Parse(reader.GetString())
                : reader.TokenType == JsonTokenType.Number
                    ? reader.GetInt64()
                    : null;
        }

        public override void Write(Utf8JsonWriter writer, BigInteger? value, JsonSerializerOptions options)
        {
            if (value != null)
                writer.WriteStringValue(value.ToString());
            else
                writer.WriteNullValue();
        }
    }
}
