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
            return sideReader.GetString() switch
            {
                "delegation" => JsonSerializer.Deserialize<DelegationContent>(ref reader, options),
                "origination" => JsonSerializer.Deserialize<OriginationContent>(ref reader, options),
                "transaction" => JsonSerializer.Deserialize<TransactionContent>(ref reader, options),
                "reveal" => JsonSerializer.Deserialize<RevealContent>(ref reader, options),
                "register_global_constant" => JsonSerializer.Deserialize<RegisterConstantContent>(ref reader, options),
                "set_deposits_limit" => JsonSerializer.Deserialize<SetDepositsLimitContent>(ref reader, options),
                _ => throw new JsonException("Invalid operation kind"),
            };
        }

        public override void Write(Utf8JsonWriter writer, ManagerOperationContent value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
