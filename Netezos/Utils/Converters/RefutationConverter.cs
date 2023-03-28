using System.Text.Json;
using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    class RefutationMoveConverter : JsonConverter<RefutationMove?>
    {
        public override RefutationMove? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var sideReader = reader;
            var refReader = reader;

            sideReader.Read();
            while (!sideReader.ValueTextEquals("refutation_kind"))
            {
                sideReader.Skip();
                sideReader.Read();
            }
            sideReader.Read();
            
            switch (sideReader.GetString())
            {
                case "start":
                    return JsonSerializer.Deserialize<RefutationStart>(ref reader, options);
                case "move":
                    refReader.Read();
                    while (!refReader.ValueTextEquals("step"))
                    {
                        refReader.Skip();
                        refReader.Read();
                    }
                    refReader.Read();
                    
                    return refReader.TokenType == JsonTokenType.StartArray
                        ? JsonSerializer.Deserialize<RefutationDissection>(ref reader, options)
                        : JsonSerializer.Deserialize<RefutationProof>(ref reader, options);
                default:
                    throw new JsonException("Invalid refutation kind");
            }
        }

        public override void Write(Utf8JsonWriter writer, RefutationMove? value, JsonSerializerOptions options)
        {
            if (value == null)
                writer.WriteNullValue();
            else
                JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
