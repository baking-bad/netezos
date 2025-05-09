﻿using Netezos.Encoding;
using Netezos.Forging.Models;
using Netezos.Utils;

namespace Netezos.Forging
{
    public partial class LocalForge
    {
        static byte[] ForgeOperation(OperationContent content)
        {
            return content switch
            {
                AttestationContent op => ForgeAttestation(op),
                PreattestationContent op => ForgePreattestation(op),
                BallotContent op => ForgeBallot(op),
                ProposalsContent op => ForgeProposals(op),
                ActivationContent op => ForgeActivation(op),
                DoubleBakingContent op => ForgeDoubleBaking(op),
                DoubleAttestationContent op => ForgeDoubleAttestation(op),
                DoublePreattestationContent op => ForgeDoublePreattestation(op),
                SeedNonceRevelationContent op => ForgeSeedNonceRevelation(op),
                VdfRevelationContent op => ForgeVdfRevelation(op),
                DrainDelegateContent op => ForgeDrainDelegate(op),
                DelegationContent op => ForgeDelegation(op),
                OriginationContent op => ForgeOrigination(op),
                TransactionContent op => ForgeTransaction(op),
                RevealContent op => ForgeReveal(op),
                RegisterConstantContent op => ForgeRegisterConstant(op),
                SetDepositsLimitContent op => ForgeSetDepositsLimit(op),
                IncreasePaidStorageContent op => ForgeIncreasePaidStorage(op),
                FailingNoopContent op => ForgeFailingNoop(op),
                TransferTicketContent op => ForgeTransferTicket(op),
                TxRollupCommitContent op => ForgeTxRollupCommit(op),
                TxRollupDispatchTicketsContent op => ForgeTxRollupDispatchTickets(op),
                TxRollupFinalizeCommitmentContent op => ForgeTxRollupFinalizeCommitment(op),
                TxRollupOriginationContent op => ForgeTxRollupOrigination(op),
                TxRollupRejectionContent op => ForgeTxRollupRejection(op),
                TxRollupRemoveCommitmentContent op => ForgeTxRollupRemoveCommitment(op),
                TxRollupReturnBondContent op => ForgeTxRollupReturnBond(op),
                TxRollupSubmitBatchContent op => ForgeTxRollupSubmitBatch(op),
                UpdateConsensusKeyContent op => ForgeUpdateConsensusKey(op),
                SrAddMessagesContent op => ForgeSrAddMessages(op),
                SrCementContent op => ForgeSrCement(op),
                SrExecuteContent op => ForgeSrExecute(op),
                SrOriginateContent op => ForgeSrOriginate(op),
                SrPublishContent op => ForgeSrPublish(op),
                SrRecoverBondContent op => ForgeSrRecoverBond(op),
                SrRefuteContent op => ForgeSrRefute(op),
                SrTmieoutContent op => ForgeSrTimeout(op),
                DalPublishCommitmentContent op => ForgeDalPublishCommitment(op),
                DalEntrapmentEvidenceContent op => ForgeDalEntrapmentEvidence(op),
                _ => throw new ArgumentException($"Invalid operation content kind {content.Kind}")
            };
        }

        static byte[] ForgeAttestation(AttestationContent operation)
        {
            if (operation.DalAttestation == null)
            {
                return Bytes.Concat(
                    ForgeTag(OperationTag.Attestation),
                    ForgeInt32(operation.Slot, 2),
                    ForgeInt32(operation.Level),
                    ForgeInt32(operation.Round),
                    Base58.Parse(operation.PayloadHash, Prefix.vh));
            }
            else
            {
                return Bytes.Concat(
                    ForgeTag(OperationTag.AttestationWithDal),
                    ForgeInt32(operation.Slot, 2),
                    ForgeInt32(operation.Level),
                    ForgeInt32(operation.Round),
                    Base58.Parse(operation.PayloadHash, Prefix.vh),
                    ForgeMicheInt(operation.DalAttestation.Value));
            }
        }

        static byte[] ForgePreattestation(PreattestationContent operation)
        {
            return Bytes.Concat(
                ForgeTag(OperationTag.Preattestation),
                ForgeInt32(operation.Slot, 2),
                ForgeInt32(operation.Level),
                ForgeInt32(operation.Round),
                Base58.Parse(operation.PayloadHash, Prefix.vh));
        }

        static byte[] ForgeBallot(BallotContent operation)
        {
            return Bytes.Concat(
                ForgeTag(OperationTag.Ballot),
                ForgeTzAddress(operation.Source),
                ForgeInt32(operation.Period),
                Base58.Parse(operation.Proposal, 2),
                new[] { (byte)operation.Ballot });
        }

        static byte[] ForgeProposals(ProposalsContent operation)
        {
            return Bytes.Concat(
                ForgeTag(OperationTag.Proposals),
                ForgeTzAddress(operation.Source),
                ForgeInt32(operation.Period),
                ForgeArray(operation.Proposals
                    .Select(x => Base58.Parse(x, 2))
                    .SelectMany(x => x)
                    .ToArray()));
        }

        static byte[] ForgeActivation(ActivationContent operation)
        {
            return Bytes.Concat(
                ForgeTag(OperationTag.Activation),
                ForgePkh(operation.Address),
                Hex.Parse(operation.Secret));
        }

        static byte[] ForgeDoubleBaking(DoubleBakingContent operation)
        {
            return Bytes.Concat(
                ForgeTag(OperationTag.DoubleBaking),
                ForgeArray(ForgeBlockHeader(operation.BlockHeader1)),
                ForgeArray(ForgeBlockHeader(operation.BlockHeader2)));
        }

        static byte[] ForgeDoubleAttestation(DoubleAttestationContent operation)
        {
            return Bytes.Concat(
                ForgeTag(OperationTag.DoubleAttestation),
                ForgeArray(ForgeInlineAttestation(operation.Op1)),
                ForgeArray(ForgeInlineAttestation(operation.Op2)));
        }

        static byte[] ForgeDoublePreattestation(DoublePreattestationContent operation)
        {
            return Bytes.Concat(
                ForgeTag(OperationTag.DoublePreattestation),
                ForgeArray(ForgeInlinePreattestation(operation.Op1)),
                ForgeArray(ForgeInlinePreattestation(operation.Op2)));
        }

        static byte[] ForgeSeedNonceRevelation(SeedNonceRevelationContent operation)
        {
            return Bytes.Concat(
                ForgeTag(OperationTag.SeedNonceRevelation),
                ForgeInt32(operation.Level),
                Hex.Parse(operation.Nonce));
        }

        static byte[] ForgeVdfRevelation(VdfRevelationContent operation)
        {
            return Bytes.Concat(
                ForgeTag(OperationTag.VdfRevelation),
                Hex.Parse(operation.Solution[0]),
                Hex.Parse(operation.Solution[1]));
        }

        static byte[] ForgeDrainDelegate(DrainDelegateContent operation)
        {
            return Bytes.Concat(
                ForgeTag(OperationTag.DrainDelegate),
                ForgeTzAddress(operation.ConsensusKey),
                ForgeTzAddress(operation.Delegate),
                ForgeTzAddress(operation.Destination));
        }

        static byte[] ForgeDelegation(DelegationContent operation)
        {
            return Bytes.Concat(
                ForgeTag(OperationTag.Delegation),
                ForgeTzAddress(operation.Source),
                ForgeMicheNat(operation.Fee),
                ForgeMicheNat(operation.Counter),
                ForgeMicheNat(operation.GasLimit),
                ForgeMicheNat(operation.StorageLimit),
                ForgeDelegate(operation.Delegate));
        }

        static byte[] ForgeOrigination(OriginationContent operation)
        {
            return Bytes.Concat(
                ForgeTag(OperationTag.Origination),
                ForgeTzAddress(operation.Source),
                ForgeMicheNat(operation.Fee),
                ForgeMicheNat(operation.Counter),
                ForgeMicheNat(operation.GasLimit),
                ForgeMicheNat(operation.StorageLimit),
                ForgeMicheNat(operation.Balance),
                ForgeDelegate(operation.Delegate),
                ForgeScript(operation.Script));
        }

        static byte[] ForgeTransaction(TransactionContent operation)
        {
            return Bytes.Concat(
                ForgeTag(OperationTag.Transaction),
                ForgeTzAddress(operation.Source),
                ForgeMicheNat(operation.Fee),
                ForgeMicheNat(operation.Counter),
                ForgeMicheNat(operation.GasLimit),
                ForgeMicheNat(operation.StorageLimit),
                ForgeMicheNat(operation.Amount),
                ForgeAddress(operation.Destination),
                ForgeParameters(operation.Parameters));
        }

        static byte[] ForgeReveal(RevealContent operation)
        {
            return Bytes.Concat(
                ForgeTag(OperationTag.Reveal),
                ForgeTzAddress(operation.Source),
                ForgeMicheNat(operation.Fee),
                ForgeMicheNat(operation.Counter),
                ForgeMicheNat(operation.GasLimit),
                ForgeMicheNat(operation.StorageLimit),
                ForgePublicKey(operation.PublicKey));
        }

        static byte[] ForgeRegisterConstant(RegisterConstantContent operation)
        {
            return Bytes.Concat(
                ForgeTag(OperationTag.RegisterConstant),
                ForgeTzAddress(operation.Source),
                ForgeMicheNat(operation.Fee),
                ForgeMicheNat(operation.Counter),
                ForgeMicheNat(operation.GasLimit),
                ForgeMicheNat(operation.StorageLimit),
                ForgeArray(ForgeMicheline(operation.Value)));
        }

        static byte[] ForgeSetDepositsLimit(SetDepositsLimitContent operation)
        {
            return Bytes.Concat(
                ForgeTag(OperationTag.SetDepositsLimit),
                ForgeTzAddress(operation.Source),
                ForgeMicheNat(operation.Fee),
                ForgeMicheNat(operation.Counter),
                ForgeMicheNat(operation.GasLimit),
                ForgeMicheNat(operation.StorageLimit),
                operation.Limit == null
                    ? ForgeBool(false)
                    : Bytes.Concat(ForgeBool(true), ForgeMicheNat(operation.Limit.Value)));
        }

        static byte[] ForgeIncreasePaidStorage(IncreasePaidStorageContent operation)
        {
            return Bytes.Concat(
                ForgeTag(OperationTag.IncreasePaidStorage),
                ForgeTzAddress(operation.Source),
                ForgeMicheNat(operation.Fee),
                ForgeMicheNat(operation.Counter),
                ForgeMicheNat(operation.GasLimit),
                ForgeMicheNat(operation.StorageLimit),
                ForgeMicheInt(operation.Amount),
                ForgeAddress(operation.Destination));
        }

        static byte[] ForgeFailingNoop(FailingNoopContent operation)
        {
            return Bytes.Concat(
                ForgeTag(OperationTag.FailingNoop),
                ForgeString(operation.Message));
        }

        static byte[] ForgeTransferTicket(TransferTicketContent operation)
        {
            return Bytes.Concat(
                ForgeTag(OperationTag.TransferTicket),
                ForgeTzAddress(operation.Source),
                ForgeMicheNat(operation.Fee),
                ForgeMicheNat(operation.Counter),
                ForgeMicheNat(operation.GasLimit),
                ForgeMicheNat(operation.StorageLimit),
                ForgeArray(ForgeMicheline(operation.TicketContent)),
                ForgeArray(ForgeMicheline(operation.TicketType)),
                ForgeAddress(operation.TicketTicketer),
                ForgeMicheNat(operation.TicketAmount),
                ForgeAddress(operation.Destination),
                ForgeString(operation.Entrypoint));
        }

        static byte[] ForgeTxRollupCommit(TxRollupCommitContent operation)
        {
            return Bytes.Concat(
                ForgeTag(OperationTag.TxRollupCommit),
                ForgeTzAddress(operation.Source),
                ForgeMicheNat(operation.Fee),
                ForgeMicheNat(operation.Counter),
                ForgeMicheNat(operation.GasLimit),
                ForgeMicheNat(operation.StorageLimit),
                Base58.Parse(operation.Rollup, Prefix.txr1),
                ForgeInt32(operation.Commitment.Level),
                ForgeArray(operation.Commitment.Messages
                    .Select(x => Base58.Parse(x, Prefix.txmr))
                    .SelectMany(x => x)
                    .ToArray()),
                operation.Commitment.Predecessor is string str
                    ? new byte[] { 1 }.Concat(Base58.Parse(str, Prefix.txc))
                    : new byte[] { 0 },
                Base58.Parse(operation.Commitment.InboxMerkleRoot, Prefix.txi));
        }

        static byte[] ForgeTxRollupDispatchTickets(TxRollupDispatchTicketsContent operation)
        {
            return Bytes.Concat(
                ForgeTag(OperationTag.TxRollupDispatchTickets),
                ForgeTzAddress(operation.Source),
                ForgeMicheNat(operation.Fee),
                ForgeMicheNat(operation.Counter),
                ForgeMicheNat(operation.GasLimit),
                ForgeMicheNat(operation.StorageLimit),
                Base58.Parse(operation.Rollup, Prefix.txr1),
                ForgeInt32(operation.Level),
                Base58.Parse(operation.ContextHash, Prefix.Co),
                ForgeInt32(operation.MessageIndex),
                ForgeArray(operation.MessageResultPath
                    .Select(x => Base58.Parse(x, Prefix.txM))
                    .SelectMany(x => x)
                    .ToArray()),
                ForgeArray(operation.TicketsInfo.SelectMany(ti => Bytes.Concat(
                    ForgeArray(ForgeMicheline(ti.Contents)),
                    ForgeArray(ForgeMicheline(ti.Type)),
                    ForgeAddress(ti.Ticketer),
                    ForgeVarLong(ti.Amount),
                    ForgeTzAddress(ti.Claimer)))
                    .ToArray()));
        }

        static byte[] ForgeTxRollupFinalizeCommitment(TxRollupFinalizeCommitmentContent operation)
        {
            return Bytes.Concat(
                ForgeTag(OperationTag.TxRollupFinalizeCommitment),
                ForgeTzAddress(operation.Source),
                ForgeMicheNat(operation.Fee),
                ForgeMicheNat(operation.Counter),
                ForgeMicheNat(operation.GasLimit),
                ForgeMicheNat(operation.StorageLimit),
                Base58.Parse(operation.Rollup, Prefix.txr1));
        }

        static byte[] ForgeTxRollupOrigination(TxRollupOriginationContent operation)
        {
            return Bytes.Concat(
                ForgeTag(OperationTag.TxRollupOrigination),
                ForgeTzAddress(operation.Source),
                ForgeMicheNat(operation.Fee),
                ForgeMicheNat(operation.Counter),
                ForgeMicheNat(operation.GasLimit),
                ForgeMicheNat(operation.StorageLimit));
        }

        static byte[] ForgeTxRollupRejection(TxRollupRejectionContent operation)
        {
            throw new NotImplementedException("Due to lack of examples no one knows how to handle it");
        }

        static byte[] ForgeTxRollupRemoveCommitment(TxRollupRemoveCommitmentContent operation)
        {
            return Bytes.Concat(
                ForgeTag(OperationTag.TxRollupRemoveCommitment),
                ForgeTzAddress(operation.Source),
                ForgeMicheNat(operation.Fee),
                ForgeMicheNat(operation.Counter),
                ForgeMicheNat(operation.GasLimit),
                ForgeMicheNat(operation.StorageLimit),
                Base58.Parse(operation.Rollup, Prefix.txr1));
        }

        static byte[] ForgeTxRollupReturnBond(TxRollupReturnBondContent operation)
        {
            return Bytes.Concat(
                ForgeTag(OperationTag.TxRollupReturnBond),
                ForgeTzAddress(operation.Source),
                ForgeMicheNat(operation.Fee),
                ForgeMicheNat(operation.Counter),
                ForgeMicheNat(operation.GasLimit),
                ForgeMicheNat(operation.StorageLimit),
                Base58.Parse(operation.Rollup, Prefix.txr1));
        }

        static byte[] ForgeTxRollupSubmitBatch(TxRollupSubmitBatchContent operation)
        {
            return Bytes.Concat(
                ForgeTag(OperationTag.TxRollupSubmitBatch),
                ForgeTzAddress(operation.Source),
                ForgeMicheNat(operation.Fee),
                ForgeMicheNat(operation.Counter),
                ForgeMicheNat(operation.GasLimit),
                ForgeMicheNat(operation.StorageLimit),
                Base58.Parse(operation.Rollup, Prefix.txr1),
                ForgeArray(operation.Content),
                operation.BurnLimit is long value
                    ? Bytes.Concat(ForgeBool(true), ForgeMicheNat(value))
                    : ForgeBool(false));
        }

        static byte[] ForgeUpdateConsensusKey(UpdateConsensusKeyContent operation)
        {
            return Bytes.Concat(
                ForgeTag(OperationTag.UpdateConsensusKey),
                ForgeTzAddress(operation.Source),
                ForgeMicheNat(operation.Fee),
                ForgeMicheNat(operation.Counter),
                ForgeMicheNat(operation.GasLimit),
                ForgeMicheNat(operation.StorageLimit),
                ForgePublicKey(operation.PublicKey),
                operation.Proof is string proof
                    ? Bytes.Concat(ForgeBool(true), ForgeSignature(proof))
                    : ForgeBool(false));
        }

        static byte[] ForgeSrAddMessages(SrAddMessagesContent operation)
        {
            return Bytes.Concat(
                ForgeTag(OperationTag.SrAddMessages),
                ForgeTzAddress(operation.Source),
                ForgeMicheNat(operation.Fee),
                ForgeMicheNat(operation.Counter),
                ForgeMicheNat(operation.GasLimit),
                ForgeMicheNat(operation.StorageLimit),
                ForgeArray(operation.Messages.Select(x => ForgeArray(x)).SelectMany(x => x).ToArray()));
        }

        static byte[] ForgeSrCement(SrCementContent operation)
        {
            return Bytes.Concat(
                ForgeTag(OperationTag.SrCement),
                ForgeTzAddress(operation.Source),
                ForgeMicheNat(operation.Fee),
                ForgeMicheNat(operation.Counter),
                ForgeMicheNat(operation.GasLimit),
                ForgeMicheNat(operation.StorageLimit),
                ForgeRollup(operation.Rollup));
        }

        static byte[] ForgeSrTimeout(SrTmieoutContent operation)
        {
            return Bytes.Concat(
                ForgeTag(OperationTag.SrTimeout),
                ForgeTzAddress(operation.Source),
                ForgeMicheNat(operation.Fee),
                ForgeMicheNat(operation.Counter),
                ForgeMicheNat(operation.GasLimit),
                ForgeMicheNat(operation.StorageLimit),
                ForgeRollup(operation.Rollup),
                ForgeTzAddress(operation.Stakers.Alice),
                ForgeTzAddress(operation.Stakers.Bob));
        }

        static byte[] ForgeSrExecute(SrExecuteContent operation)
        {
            return Bytes.Concat(
                ForgeTag(OperationTag.SrExecute),
                ForgeTzAddress(operation.Source),
                ForgeMicheNat(operation.Fee),
                ForgeMicheNat(operation.Counter),
                ForgeMicheNat(operation.GasLimit),
                ForgeMicheNat(operation.StorageLimit),
                ForgeRollup(operation.Rollup),
                ForgeCommitment(operation.Commitment),
                ForgeArray(operation.OutputProof));
        }

        static byte[] ForgeSrOriginate(SrOriginateContent operation)
        {
            return Bytes.Concat(
                ForgeTag(OperationTag.SrOriginate),
                ForgeTzAddress(operation.Source),
                ForgeMicheNat(operation.Fee),
                ForgeMicheNat(operation.Counter),
                ForgeMicheNat(operation.GasLimit),
                ForgeMicheNat(operation.StorageLimit),
                new[] { (byte)operation.PvmKind },
                ForgeArray(operation.Kernel),
                ForgeArray(operation.OriginationProof),
                ForgeArray(ForgeMicheline(operation.ParametersType)));
        }

        static byte[] ForgeSrPublish(SrPublishContent operation)
        {
            return Bytes.Concat(
                ForgeTag(OperationTag.SrPublish),
                ForgeTzAddress(operation.Source),
                ForgeMicheNat(operation.Fee),
                ForgeMicheNat(operation.Counter),
                ForgeMicheNat(operation.GasLimit),
                ForgeMicheNat(operation.StorageLimit),
                ForgeRollup(operation.Rollup),
                ForgeCommitment(operation.Commitment));
        }

        static byte[] ForgeSrRecoverBond(SrRecoverBondContent operation)
        {
            return Bytes.Concat(
                ForgeTag(OperationTag.SrRecoverBond),
                ForgeTzAddress(operation.Source),
                ForgeMicheNat(operation.Fee),
                ForgeMicheNat(operation.Counter),
                ForgeMicheNat(operation.GasLimit),
                ForgeMicheNat(operation.StorageLimit),
                ForgeRollup(operation.Rollup),
                ForgeTzAddress(operation.Staker));
        }

        static byte[] ForgeSrRefute(SrRefuteContent operation)
        {
            return Bytes.Concat(
                ForgeTag(OperationTag.SrRefute),
                ForgeTzAddress(operation.Source),
                ForgeMicheNat(operation.Fee),
                ForgeMicheNat(operation.Counter),
                ForgeMicheNat(operation.GasLimit),
                ForgeMicheNat(operation.StorageLimit),
                ForgeRollup(operation.Rollup),
                ForgeTzAddress(operation.Opponent),
                ForgeRefutation(operation.Refutation));
        }

        static byte[] ForgeDalPublishCommitment(DalPublishCommitmentContent operation)
        {
            return Bytes.Concat(
                ForgeTag(OperationTag.DalPublishCommitment),
                ForgeTzAddress(operation.Source),
                ForgeMicheNat(operation.Fee),
                ForgeMicheNat(operation.Counter),
                ForgeMicheNat(operation.GasLimit),
                ForgeMicheNat(operation.StorageLimit),
                [operation.SlotHeader.SlotIndex],
                Base58.Parse(operation.SlotHeader.Commitment, Prefix.sh),
                operation.SlotHeader.CommitmentProof);
        }

        static byte[] ForgeDalEntrapmentEvidence(DalEntrapmentEvidenceContent operation)
        {
            return Bytes.Concat(
                ForgeTag(OperationTag.DalEntrapmentEvidence),
                ForgeArray(ForgeInlineAttestation(operation.Attestation)),
                ForgeInt32(operation.SlotIndex, 1),
                ForgeInt32(operation.ShardWithProof.Shard.Id),
                ForgeArray(operation.ShardWithProof.Shard.Hashes.Select(x => Hex.Parse(x)).SelectMany(x => x).ToArray()),
                Base58.Parse(operation.ShardWithProof.Proof, 3)
            );
        }

        #region nested
        static byte[] ForgeBlockHeader(BlockHeader header)
        {
            return Bytes.Concat(
                ForgeInt32(header.Level),
                ForgeInt32(header.Proto, 1),
                Base58.Parse(header.Predecessor, 2),
                ForgeInt64(header.Timestamp.ToUnixTime()),
                ForgeInt32(header.ValidationPass, 1),
                Base58.Parse(header.OperationsHash, 3),
                ForgeArray(header.Fitness.Select(x => ForgeArray(Hex.Parse(x))).SelectMany(x => x).ToArray()),
                Base58.Parse(header.Context, 2),
                Base58.Parse(header.PayloadHash, Prefix.vh),
                ForgeInt32(header.PayloadRound),
                Hex.Parse(header.ProofOfWorkNonce),
                ForgeSeedNonce(header.SeedNonceHash),
                new[] { (byte)header.LiquidityBakingToggleVote },
                Base58.Parse(header.Signature, 3));
        }

        static byte[] ForgeInlineAttestation(InlineAttestation op)
        {
            return Bytes.Concat(
                Base58.Parse(op.Branch, 2),
                ForgeAttestation(op.Operations),
                Base58.Parse(op.Signature, 3));
        }

        static byte[] ForgeInlinePreattestation(InlinePreattestation op)
        {
            return Bytes.Concat(
                Base58.Parse(op.Branch, 2),
                ForgePreattestation(op.Operations),
                Base58.Parse(op.Signature, 3));
        }

        static byte[] ForgeSeedNonce(string? nonce)
        {
            return nonce == null ? ForgeBool(false) : Bytes.Concat(
                ForgeBool(true),
                Base58.Parse(nonce, 3));
        }

        static byte[] ForgeDelegate(string? baker)
        {
            return baker == null ? ForgeBool(false) : Bytes.Concat(
                ForgeBool(true),
                ForgeTzAddress(baker));
        }

        static byte[] ForgeParameters(Parameters? param)
        {
            return param == null ? ForgeBool(false) : Bytes.Concat(
                ForgeBool(true),
                ForgeEntrypoint(param.Entrypoint),
                ForgeArray(ForgeMicheline(param.Value).ToArray()));
        }

        static byte[] ForgeScript(Script script)
        {
            return Bytes.Concat(
                ForgeArray(ForgeMicheline(script.Code)),
                ForgeArray(ForgeMicheline(script.Storage)));
        }

        static byte[] ForgeVarLong(long value)
        {
            if (value <= byte.MaxValue) return new byte[] { 0, (byte)value };
            if (value <= ushort.MaxValue) return new byte[] { 1 }.Concat(ForgeInt64(value, 2));
            if (value <= int.MaxValue) return new byte[] { 2 }.Concat(ForgeInt64(value, 4));
            return new byte[] { 3 }.Concat(ForgeInt64(value, 8));
        }

        static byte[] ForgeCommitment(Commitment commitment)
        {
            return Bytes.Concat(
                ForgeCommitment(commitment.State),
                ForgeInt32(commitment.InboxLevel),
                ForgeCommitment(commitment.Predecessor),
                ForgeInt64(commitment.Ticks));
        }

        static byte[] ForgeRefutation(RefutationMove refutation)
        {
            return refutation switch
            {
                RefutationStart start => ForgeRefutationStart(start),
                RefutationDissection move => ForgeRefutationDissectionMove(move),
                RefutationProof move => ForgeRefutationProofMove(move),
                _ => throw new ArgumentException("Invalid refutation move type")
            };
        }

        static byte[] ForgeRefutationStart(RefutationStart start)
        {
            return Bytes.Concat(
                new byte[] { 0 },
                ForgeCommitment(start.PlayerCommitment),
                ForgeCommitment(start.OpponentCommitment)
            );
        }

        static byte[] ForgeRefutationDissectionMove(RefutationDissection move)
        {
            return Bytes.Concat(
                new byte[] { 1 },
                ForgeMicheNat(move.Choice),
                new byte[] { 0 },
                ForgeArray(move.Steps.Select(x =>
                    Bytes.Concat(
                         x.State == null
                             ? ForgeBool(false)
                             : ForgeBool(true).Concat(
                                 ForgeCommitment(x.State)),
                        ForgeMicheNat(x.Tick))
                ).SelectMany(x => x).ToArray()));
        }

        static byte[] ForgeRefutationProofMove(RefutationProof move)
        {
            return Bytes.Concat(
                new byte[] { 1 },
                ForgeMicheNat(move.Choice),
                new byte[] { 1 },
                ForgeArray(move.Step.PvmStep),
                move.Step.InputProof == null ? ForgeBool(false) : Bytes.Concat(
                    ForgeBool(true),
                    move.Step.InputProof switch
                    {
                        InboxProof inbox => ForgeInboxProof(inbox),
                        RevealProof reveal => ForgeRevealProof(reveal),
                        FirstInputProof => new byte[] { 2 },
                        _ => throw new ArgumentException("Invalid input proof type")
                    })
            );
        }

        static byte[] ForgeInboxProof(InboxProof inbox)
        {
            return Bytes.Concat(
                new byte[] { 0 },
                ForgeInt32(inbox.Level),
                ForgeMicheNat(inbox.MessageCounter),
                ForgeArray(inbox.Proof));
        }

        static byte[] ForgeRevealProof(RevealProof reveal)
        {
            return Bytes.Concat(
                new byte[] { 1 },
                reveal.Proof switch
                {
                    RawDataProof raw => Bytes.Concat(
                        new byte[] { 0 },
                        ForgeArray(raw.RawData, 2)),
                    MetadataProof =>
                        new byte[] { 1 },
                    DalPageProof dal => Bytes.Concat(
                        new byte[] { 2 },
                        ForgeDalPageId(dal.DalPageId),
                        ForgeArray(dal.Proof)),
                    _ => throw new ArgumentException("Invalid reveal proof type")
                });
        }

        static byte[] ForgeDalPageId(DalPageId id)
        {
            return Bytes.Concat(
                ForgeInt32(id.PublishedLevel, 4),
                ForgeInt32(id.SlotIndex, 1),
                ForgeInt32(id.PageIndex, 2)
            );
        }
        #endregion
    }
}
