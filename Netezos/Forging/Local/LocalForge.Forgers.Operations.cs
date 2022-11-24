using System;
using System.Linq;
using Netezos.Encoding;
using Netezos.Forging.Models;
using Netezos.Utils;

namespace Netezos.Forging
{
    public partial class LocalForge
    {
        static byte[] ForgeOperation(OperationContent content)
        {
            switch (content)
            {
                case EndorsementContent op:
                    return ForgeEndorsement(op);
                case PreendorsementContent op:
                    return ForgePreendorsement(op);
                case BallotContent op:
                    return ForgeBallot(op);
                case ProposalsContent op:
                    return ForgeProposals(op);
                case ActivationContent op:
                    return ForgeActivation(op);
                case DoubleBakingContent op:
                    return ForgeDoubleBaking(op);
                case DoubleEndorsementContent op:
                    return ForgeDoubleEndorsement(op);
                case DoublePreendorsementContent op:
                    return ForgeDoublePreendorsement(op);
                case SeedNonceRevelationContent op:
                    return ForgeSeedNonceRevelaion(op);
                case VdfRevelationContent op:
                    return ForgeVdfRevelaion(op);
                case DrainDelegateContent op:
                    return ForgeDrainDelegate(op);
                case DelegationContent op:
                    return ForgeDelegation(op);
                case OriginationContent op:
                    return ForgeOrigination(op);
                case TransactionContent op:
                    return ForgeTransaction(op);
                case RevealContent op:
                    return ForgeReveal(op);
                case RegisterConstantContent op:
                    return ForgeRegisterConstant(op);
                case SetDepositsLimitContent op:
                    return ForgeSetDepositsLimit(op);
                case IncreasePaidStorageContent op:
                    return ForgeIncreasePaidStorage(op);
                case FailingNoopContent op:
                    return ForgeFailingNoop(op);
                case TransferTicketContent op:
                    return ForgeTransferTicket(op);
                case TxRollupCommitContent op:
                    return ForgeTxRollupCommit(op);
                case TxRollupDispatchTicketsContent op:
                    return ForgeTxRollupDispatchTickets(op);
                case TxRollupFinalizeCommitmentContent op:
                    return ForgeTxRollupFinalizeCommitment(op);
                case TxRollupOriginationContent op:
                    return ForgeTxRollupOrigination(op);
                case TxRollupRejectionContent op:
                    return ForgeTxRollupRejection(op);
                case TxRollupRemoveCommitmentContent op:
                    return ForgeTxRollupRemoveCommitment(op);
                case TxRollupReturnBondContent op:
                    return ForgeTxRollupReturnBond(op);
                case TxRollupSubmitBatchContent op:
                    return ForgeTxRollupSubmitBatch(op);
                case UpdateConsensusKeyContent op:
                    return ForgeUpdateConsensusKey(op);
                default:
                    throw new ArgumentException($"Invalid operation content kind {content.Kind}");
            }
        }

        static byte[] ForgeEndorsement(EndorsementContent operation)
        {
            return Bytes.Concat(
                ForgeTag(OperationTag.Endorsement),
                ForgeInt32(operation.Slot, 2),
                ForgeInt32(operation.Level),
                ForgeInt32(operation.Round),
                Base58.Parse(operation.PayloadHash, Prefix.vh));
        }

        static byte[] ForgePreendorsement(PreendorsementContent operation)
        {
            return Bytes.Concat(
                ForgeTag(OperationTag.Preendorsement),
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

        static byte[] ForgeDoubleEndorsement(DoubleEndorsementContent operation)
        {
            return Bytes.Concat(
                ForgeTag(OperationTag.DoubleEndorsement),
                ForgeArray(ForgeInlinedEndorsement(operation.Op1)),
                ForgeArray(ForgeInlinedEndorsement(operation.Op2)));
        }

        static byte[] ForgeDoublePreendorsement(DoublePreendorsementContent operation)
        {
            return Bytes.Concat(
                ForgeTag(OperationTag.DoublePreendorsement),
                ForgeArray(ForgeInlinedPreendorsement(operation.Op1)),
                ForgeArray(ForgeInlinedPreendorsement(operation.Op2)));
        }

        static byte[] ForgeSeedNonceRevelaion(SeedNonceRevelationContent operation)
        {
            return Bytes.Concat(
                ForgeTag(OperationTag.SeedNonceRevelation),
                ForgeInt32(operation.Level),
                Hex.Parse(operation.Nonce));
        }

        static byte[] ForgeVdfRevelaion(VdfRevelationContent operation)
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
                ForgePublicKey(operation.PublicKey));
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

        static byte[] ForgeInlinedEndorsement(InlinedEndorsement op)
        {
            return Bytes.Concat(
                Base58.Parse(op.Branch, 2),
                ForgeEndorsement(op.Operations),
                Base58.Parse(op.Signature, 3));
        }

        static byte[] ForgeInlinedPreendorsement(InlinedPreendorsement op)
        {
            return Bytes.Concat(
                Base58.Parse(op.Branch, 2),
                ForgePreendorsement(op.Operations),
                Base58.Parse(op.Signature, 3));
        }

        static byte[] ForgeSeedNonce(string nonce)
        {
            return nonce == null ? ForgeBool(false) : Bytes.Concat(
                ForgeBool(true),
                Base58.Parse(nonce, 3));
        }

        static byte[] ForgeDelegate(string delegat)
        {
            return delegat == null ? ForgeBool(false) : Bytes.Concat(
                ForgeBool(true),
                ForgeTzAddress(delegat));
        }

        static byte[] ForgeParameters(Parameters param)
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

        #endregion
    }
}
