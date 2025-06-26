using System.Text.Json;
using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    class ManagerOperationContentConverter : JsonConverter<ManagerOperationContent?>
    {
        public override ManagerOperationContent? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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
                "increase_paid_storage" => JsonSerializer.Deserialize<IncreasePaidStorageContent>(ref reader, options),
                "transfer_ticket" => JsonSerializer.Deserialize<TransferTicketContent>(ref reader, options),
                "update_consensus_key" => JsonSerializer.Deserialize<UpdateConsensusKeyContent>(ref reader, options),
                "smart_rollup_add_messages" => JsonSerializer.Deserialize<SrAddMessagesContent>(ref reader, options),
                "smart_rollup_cement" => JsonSerializer.Deserialize<SrCementContent>(ref reader, options),
                "smart_rollup_timeout" => JsonSerializer.Deserialize<SrTmieoutContent>(ref reader, options),
                "smart_rollup_execute_outbox_message" => JsonSerializer.Deserialize<SrExecuteContent>(ref reader, options),
                "smart_rollup_originate" => JsonSerializer.Deserialize<SrOriginateContent>(ref reader, options),
                "smart_rollup_publish" => JsonSerializer.Deserialize<SrPublishContent>(ref reader, options),
                "smart_rollup_recover_bond" => JsonSerializer.Deserialize<SrRecoverBondContent>(ref reader, options),
                "smart_rollup_refute" => JsonSerializer.Deserialize<SrRefuteContent>(ref reader, options),
                "dal_publish_commitment" => JsonSerializer.Deserialize<DalPublishCommitmentContent>(ref reader, options),
                _ => throw new JsonException("Invalid operation kind"),
            };
        }

        public override void Write(Utf8JsonWriter writer, ManagerOperationContent? value, JsonSerializerOptions options)
        {
            if (value == null)
                writer.WriteNullValue();
            else
                JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
