using System.Text.Json;
using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    class InputProofConverter : JsonConverter<InputProof?>
    {
        public override InputProof? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var sideReader = reader;

            sideReader.Read();
            while (!sideReader.ValueTextEquals("input_proof_kind"))
            {
                sideReader.Skip();
                sideReader.Read();
            }
            sideReader.Read();
            
            return sideReader.GetString() switch
            {
                "inbox_proof" => JsonSerializer.Deserialize<InboxProof>(ref reader, options),
                "reveal_proof" => throw new NotImplementedException(),
                "first_input" => throw new NotImplementedException(),
                _ => throw new JsonException("Invalid input proof kind"),
            };
        }

        public override void Write(Utf8JsonWriter writer, InputProof? value, JsonSerializerOptions options)
        {
            if (value == null)
                writer.WriteNullValue();
            else
                JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}