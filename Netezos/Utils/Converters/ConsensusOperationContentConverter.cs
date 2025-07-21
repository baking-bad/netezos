using System.Text.Json;
using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    class ConsensusOperationContentConverter : JsonConverter<ConsensusOperationContent?>
    {
        public override ConsensusOperationContent? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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
                "attestation" => JsonSerializer.Deserialize<AttestationContent>(ref reader, options),
                "attestation_with_dal" => JsonSerializer.Deserialize<AttestationWithDalContent>(ref reader, options),
                "attestations_aggregate" => JsonSerializer.Deserialize<AttestationsAggregateContent>(ref reader, options),
                "preattestation" => JsonSerializer.Deserialize<PreattestationContent>(ref reader, options),
                "preattestations_aggregate" => JsonSerializer.Deserialize<PreattestationsAggregateContent>(ref reader, options),
                _ => throw new JsonException("Invalid consensus operation kind"),
            };
        }

        public override void Write(Utf8JsonWriter writer, ConsensusOperationContent? value, JsonSerializerOptions options)
        {
            if (value == null)
                writer.WriteNullValue();
            else
                JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
