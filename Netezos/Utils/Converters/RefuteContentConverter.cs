using System.Text.Json;
using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    class RefutationConverter : JsonConverter<Refutation?>
    {
        public override Refutation? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var sideReader = reader;

            sideReader.Read();
            while (!sideReader.ValueTextEquals("refutation_kind"))
            {
                sideReader.Skip();
                sideReader.Read();
            }

            sideReader.Read();
            return sideReader.GetString() switch
            {
                "start" => JsonSerializer.Deserialize<RefutationStart>(ref reader, options),
                "move" => JsonSerializer.Deserialize<RefutationMove>(ref reader, options),
                _ => throw new JsonException("Invalid refutation kind"),
            };
        }

        public override void Write(Utf8JsonWriter writer, Refutation? value, JsonSerializerOptions options)
        {
            if (value == null)
                writer.WriteNullValue();
            else
                JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
