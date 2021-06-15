using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    class ManagerOperationContentConverter : JsonConverter<ManagerOperationContent>
    {
        public override ManagerOperationContent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var sideReader = reader;

            sideReader.Read();
            while (!sideReader.ValueTextEquals("kind"))
            {
                sideReader.Skip();
                sideReader.Read();
            }

            sideReader.Read();
            var kind = sideReader.GetString();

            switch (kind)
            {
                case "delegation": return JsonSerializer.Deserialize<DelegationContent>(ref reader, options);
                case "origination": return JsonSerializer.Deserialize<OriginationContent>(ref reader, options);
                case "transaction": return JsonSerializer.Deserialize<TransactionContent>(ref reader, options);
                case "reveal": return JsonSerializer.Deserialize<RevealContent>(ref reader, options);
                default: throw new JsonException("Invalid operation kind");
            }
        }

        public override void Write(Utf8JsonWriter writer, ManagerOperationContent value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
