using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    class OperationContentConverter : JsonConverter<OperationContent>
    {
        public override OperationContent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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
                case "endorsement": return JsonSerializer.Deserialize<EndorsementContent>(ref reader, options);
                case "endorsement_with_slot": return JsonSerializer.Deserialize<EndorsementWithSlotContent>(ref reader, options);
                case "ballot": return JsonSerializer.Deserialize<BallotContent>(ref reader, options);
                case "proposals": return JsonSerializer.Deserialize<ProposalsContent>(ref reader, options);
                case "activate_account": return JsonSerializer.Deserialize<ActivationContent>(ref reader, options);
                case "double_baking_evidence": return JsonSerializer.Deserialize<DoubleBakingContent>(ref reader, options);
                case "double_endorsement_evidence": return JsonSerializer.Deserialize<DoubleEndorsementContent>(ref reader, options);
                case "seed_nonce_revelation": return JsonSerializer.Deserialize<SeedNonceRevelationContent>(ref reader, options);
                case "delegation": return JsonSerializer.Deserialize<DelegationContent>(ref reader, options);
                case "origination": return JsonSerializer.Deserialize<OriginationContent>(ref reader, options);
                case "transaction": return JsonSerializer.Deserialize<TransactionContent>(ref reader, options);
                case "reveal": return JsonSerializer.Deserialize<RevealContent>(ref reader, options);
                default: throw new JsonException("Invalid operation kind");
            }
        }

        public override void Write(Utf8JsonWriter writer, OperationContent value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
