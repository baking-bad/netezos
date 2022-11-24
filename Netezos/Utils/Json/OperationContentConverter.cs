﻿using System;
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
                "tx_rollup_commit" => JsonSerializer.Deserialize<TxRollupCommitContent>(ref reader, options),
                "tx_rollup_dispatch_tickets" => JsonSerializer.Deserialize<TxRollupDispatchTicketsContent>(ref reader, options),
                "tx_rollup_finalize_commitment" => JsonSerializer.Deserialize<TxRollupFinalizeCommitmentContent>(ref reader, options),
                "tx_rollup_origination" => JsonSerializer.Deserialize<TxRollupOriginationContent>(ref reader, options),
                "tx_rollup_rejection" => JsonSerializer.Deserialize<TxRollupRejectionContent>(ref reader, options),
                "tx_rollup_remove_commitment" => JsonSerializer.Deserialize<TxRollupRemoveCommitmentContent>(ref reader, options),
                "tx_rollup_return_bond" => JsonSerializer.Deserialize<TxRollupReturnBondContent>(ref reader, options),
                "tx_rollup_submit_batch" => JsonSerializer.Deserialize<TxRollupSubmitBatchContent>(ref reader, options),
                "update_consensus_key" => JsonSerializer.Deserialize<UpdateConsensusKeyContent>(ref reader, options),
                _ => throw new JsonException("Invalid operation kind"),
            };
        }

        public override void Write(Utf8JsonWriter writer, OperationContent value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
