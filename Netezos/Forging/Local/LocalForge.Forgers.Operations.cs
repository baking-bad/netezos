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
                case EndorsementContent endorsement:
                    return ForgeEndorsement(endorsement);
                case PreendorsementContent preendorsement:
                    return ForgePreendorsement(preendorsement);
                case BallotContent ballot:
                    return ForgeBallot(ballot);
                case ProposalsContent proposals:
                    return ForgeProposals(proposals);
                case ActivationContent activation:
                    return ForgeActivation(activation);
                case DoubleBakingContent doubleBaking:
                    return ForgeDoubleBaking(doubleBaking);
                case DoubleEndorsementContent doubleEndorsement:
                    return ForgeDoubleEndorsement(doubleEndorsement);
                case DoublePreendorsementContent doublePreendorsement:
                    return ForgeDoublePreendorsement(doublePreendorsement);
                case SeedNonceRevelationContent seed:
                    return ForgeSeedNonceRevelaion(seed);
                case DelegationContent delegation:
                    return ForgeDelegation(delegation);
                case OriginationContent origination:
                    return ForgeOrigination(origination);
                case TransactionContent transaction:
                    return ForgeTransaction(transaction);
                case RevealContent reveal:
                    return ForgeReveal(reveal);
                case RegisterConstantContent registerConstant:
                    return ForgeRegisterConstant(registerConstant);
                case SetDepositsLimitContent setDepositsLimit:
                    return ForgeSetDepositsLimit(setDepositsLimit);
                default:
                    throw new ArgumentException($"Invalid operation content kind {content.Kind}");
            }
        }

        static byte[] ForgeEndorsement(EndorsementContent operation)
        {
            return Bytes.Concat(
                ForgeMicheNat((int)OperationTag.Endorsement),
                ForgeInt32(operation.Slot, 2),
                ForgeInt32(operation.Level),
                ForgeInt32(operation.Round),
                Base58.Parse(operation.PayloadHash, Prefix.vh));
        }

        static byte[] ForgePreendorsement(PreendorsementContent operation)
        {
            return Bytes.Concat(
                ForgeMicheNat((int)OperationTag.Preendorsement),
                ForgeInt32(operation.Slot, 2),
                ForgeInt32(operation.Level),
                ForgeInt32(operation.Round),
                Base58.Parse(operation.PayloadHash, Prefix.vh));
        }

        static byte[] ForgeBallot(BallotContent operation)
        {
            return Bytes.Concat(
                ForgeMicheNat((int)OperationTag.Ballot),
                ForgeTzAddress(operation.Source),
                ForgeInt32(operation.Period),
                Base58.Parse(operation.Proposal, 2),
                new[] { (byte)operation.Ballot });
        }

        static byte[] ForgeProposals(ProposalsContent operation)
        {
            return Bytes.Concat(
                ForgeMicheNat((int)OperationTag.Proposals),
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
                ForgeMicheNat((int)OperationTag.Activation),
                ForgeTz1Address(operation.Address),
                Hex.Parse(operation.Secret));
        }

        static byte[] ForgeDoubleBaking(DoubleBakingContent operation)
        {
            return Bytes.Concat(
                ForgeMicheNat((int)OperationTag.DoubleBaking),
                ForgeArray(ForgeBlockHeader(operation.BlockHeader1)),
                ForgeArray(ForgeBlockHeader(operation.BlockHeader2)));
        }

        static byte[] ForgeDoubleEndorsement(DoubleEndorsementContent operation)
        {
            return Bytes.Concat(
                ForgeMicheNat((int)OperationTag.DoubleEndorsement),
                ForgeArray(ForgeInlinedEndorsement(operation.Op1)),
                ForgeArray(ForgeInlinedEndorsement(operation.Op2)));
        }

        static byte[] ForgeDoublePreendorsement(DoublePreendorsementContent operation)
        {
            return Bytes.Concat(
                ForgeMicheNat((int)OperationTag.DoublePreendorsement),
                ForgeArray(ForgeInlinedPreendorsement(operation.Op1)),
                ForgeArray(ForgeInlinedPreendorsement(operation.Op2)));
        }

        static byte[] ForgeSeedNonceRevelaion(SeedNonceRevelationContent operation)
        {
            return Bytes.Concat(
                ForgeMicheNat((int)OperationTag.SeedNonceRevelation),
                ForgeInt32(operation.Level),
                Hex.Parse(operation.Nonce));
        }

        static byte[] ForgeDelegation(DelegationContent operation)
        {
            return Bytes.Concat(
                ForgeMicheNat((int)OperationTag.Delegation),
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
                ForgeMicheNat((int)OperationTag.Origination),
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
                ForgeMicheNat((int)OperationTag.Transaction),
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
                ForgeMicheNat((int)OperationTag.Reveal),
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
                ForgeMicheNat((int)OperationTag.RegisterConstant),
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
                ForgeMicheNat((int)OperationTag.SetDepositsLimit),
                ForgeTzAddress(operation.Source),
                ForgeMicheNat(operation.Fee),
                ForgeMicheNat(operation.Counter),
                ForgeMicheNat(operation.GasLimit),
                ForgeMicheNat(operation.StorageLimit),
                operation.Limit == null
                    ? ForgeBool(false)
                    : Bytes.Concat(ForgeBool(true), ForgeMicheNat(operation.Limit.Value)));
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
                ForgeInt32(header.Priority, 2),
                Hex.Parse(header.ProofOfWorkNonce),
                ForgeSeedNonce(header.SeedNonceHash),
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
        #endregion
    }
}
