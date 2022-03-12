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
            return sideReader.GetString() switch
            {
                "endorsement" => JsonSerializer.Deserialize<EndorsementContent>(ref reader, options),
                "preendorsement" => JsonSerializer.Deserialize<PreendorsementContent>(ref reader, options),
                "ballot" => JsonSerializer.Deserialize<BallotContent>(ref reader, options),
                "proposals" => JsonSerializer.Deserialize<ProposalsContent>(ref reader, options),
                "activate_account" => JsonSerializer.Deserialize<ActivationContent>(ref reader, options),
                "double_baking_evidence" => JsonSerializer.Deserialize<DoubleBakingContent>(ref reader, options),
                "double_endorsement_evidence" => JsonSerializer.Deserialize<DoubleEndorsementContent>(ref reader, options),
                "double_preendorsement_evidence" => JsonSerializer.Deserialize<DoublePreendorsementContent>(ref reader, options),
                "seed_nonce_revelation" => JsonSerializer.Deserialize<SeedNonceRevelationContent>(ref reader, options),
                "delegation" => JsonSerializer.Deserialize<DelegationContent>(ref reader, options),
                "origination" => JsonSerializer.Deserialize<OriginationContent>(ref reader, options),
                "transaction" => JsonSerializer.Deserialize<TransactionContent>(ref reader, options),
                "reveal" => JsonSerializer.Deserialize<RevealContent>(ref reader, options),
                "register_global_constant" => JsonSerializer.Deserialize<RegisterConstantContent>(ref reader, options),
                "set_deposits_limit" => JsonSerializer.Deserialize<SetDepositsLimitContent>(ref reader, options),
                _ => throw new JsonException("Invalid operation kind"),
            };
        }

        public override void Write(Utf8JsonWriter writer, OperationContent value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
