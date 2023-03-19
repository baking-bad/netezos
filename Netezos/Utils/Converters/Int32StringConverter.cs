using System.Text.Json;
using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    class Int32StringConverter : JsonConverter<int>
    {
        public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType == JsonTokenType.String
                ? int.Parse(reader.GetString() ?? throw new FormatException("Cannot read from null"))
                : reader.GetInt32();
        }

        public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
