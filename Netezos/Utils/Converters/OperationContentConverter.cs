using System.Text.Json;
using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    class OperationContentConverter : JsonConverter<OperationContent?>
    {
        public override OperationContent? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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
                "ballot" => JsonSerializer.Deserialize<BallotContent>(ref reader, options),
                "proposals" => JsonSerializer.Deserialize<ProposalsContent>(ref reader, options),
                "activate_account" => JsonSerializer.Deserialize<ActivationContent>(ref reader, options),
                "double_baking_evidence" => JsonSerializer.Deserialize<DoubleBakingContent>(ref reader, options),
                "double_consensus_operation_evidence" => JsonSerializer.Deserialize<DoubleConsensusContent>(ref reader, options),
                "seed_nonce_revelation" => JsonSerializer.Deserialize<SeedNonceRevelationContent>(ref reader, options),
                "vdf_revelation" => JsonSerializer.Deserialize<VdfRevelationContent>(ref reader, options),
                "drain_delegate" => JsonSerializer.Deserialize<DrainDelegateContent>(ref reader, options),
                "delegation" => JsonSerializer.Deserialize<DelegationContent>(ref reader, options),
                "origination" => JsonSerializer.Deserialize<OriginationContent>(ref reader, options),
                "transaction" => JsonSerializer.Deserialize<TransactionContent>(ref reader, options),
                "reveal" => JsonSerializer.Deserialize<RevealContent>(ref reader, options),
                "register_global_constant" => JsonSerializer.Deserialize<RegisterConstantContent>(ref reader, options),
                "set_deposits_limit" => JsonSerializer.Deserialize<SetDepositsLimitContent>(ref reader, options),
                "increase_paid_storage" => JsonSerializer.Deserialize<IncreasePaidStorageContent>(ref reader, options),
                "failing_noop" => JsonSerializer.Deserialize<FailingNoopContent>(ref reader, options),
                "transfer_ticket" => JsonSerializer.Deserialize<TransferTicketContent>(ref reader, options),
                "update_companion_key" => JsonSerializer.Deserialize<UpdateCompanionKeyContent>(ref reader, options),
                "update_consensus_key" => JsonSerializer.Deserialize<UpdateConsensusKeyContent>(ref reader, options),
                "smart_rollup_add_messages" => JsonSerializer.Deserialize<SrAddMessagesContent>(ref reader, options),
                "smart_rollup_cement" => JsonSerializer.Deserialize<SrCementContent>(ref reader, options),
                "smart_rollup_timeout" => JsonSerializer.Deserialize<SrTimeoutContent>(ref reader, options),
                "smart_rollup_execute_outbox_message" => JsonSerializer.Deserialize<SrExecuteContent>(ref reader, options),
                "smart_rollup_originate" => JsonSerializer.Deserialize<SrOriginateContent>(ref reader, options),
                "smart_rollup_publish" => JsonSerializer.Deserialize<SrPublishContent>(ref reader, options),
                "smart_rollup_recover_bond" => JsonSerializer.Deserialize<SrRecoverBondContent>(ref reader, options),
                "smart_rollup_refute" => JsonSerializer.Deserialize<SrRefuteContent>(ref reader, options),
                "dal_publish_commitment" => JsonSerializer.Deserialize<DalPublishCommitmentContent>(ref reader, options),
                "dal_entrapment_evidence" => JsonSerializer.Deserialize<DalEntrapmentEvidenceContent>(ref reader, options),
                _ => throw new JsonException("Invalid operation kind"),
            };
        }

        public override void Write(Utf8JsonWriter writer, OperationContent? value, JsonSerializerOptions options)
        {
            if (value == null)
                writer.WriteNullValue();
            else
                JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
