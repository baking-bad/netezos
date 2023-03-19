using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    class BigIntegerStringConverter : JsonConverter<BigInteger>
    {
        public override BigInteger Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return BigInteger.Parse(reader.GetString() ?? throw new FormatException("Cannot read from null"));
        }

        public override void Write(Utf8JsonWriter writer, BigInteger value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
