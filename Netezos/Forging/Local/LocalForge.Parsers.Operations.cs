using System;
using System.Linq;
using Netezos.Encoding;
using Netezos.Forging.Models;

namespace Netezos.Forging
{
    public partial class LocalParse : IOperationParser
    {
        static byte[] ParseOperation(OperationContent content)
        {
            switch (content)
            {
                case EndorsementContent endorsement:
                    return ParseEndorsement(endorsement);
                case BallotContent ballot:
                    return ParseBallot(ballot);
                case ProposalsContent proposals:
                    return ParseProposals(proposals);
                case ActivationContent activation:
                    return ParseActivation(activation);
                case DoubleBakingContent doubleBaking:
                    return ParseDoubleBaking(doubleBaking);
                case DoubleEndorsementContent doubleEndorsement:
                    return ParseDoubleEndorsement(doubleEndorsement);
                case SeedNonceRevelationContent seed:
                    return ParseSeedNonceRevelaion(seed);
                case DelegationContent delegation:
                    return ParseDelegation(delegation);
                case OriginationContent origination:
                    return ParseOrigination(origination);
                case TransactionContent transaction:
                    return ParseTransaction(transaction);
                case RevealContent reveal:
                    return ParseReveal(reveal);
                default:
                    throw new ArgumentException($"Invalid operation content kind {content.Kind}");
            }
        }

        static byte[] ParseEndorsement(EndorsementContent operation)
        {
            return Concat(
                ParseMicheNat((int)OperationTag.Endorsement),
                ParseInt32(operation.Level));
        }

        static byte[] ParseBallot(BallotContent operation)
        {
            return Concat(
                ParseMicheNat((int)OperationTag.Ballot),
                ParseTzAddress(operation.Source),
                ParseInt32(operation.Period),
                Base58.Parse(operation.Proposal, 2),
                new[] { (byte)operation.Ballot });
        }

        static byte[] ParseProposals(ProposalsContent operation)
        {
            return Concat(
                ParseMicheNat((int)OperationTag.Proposals),
                ParseTzAddress(operation.Source),
                ParseInt32(operation.Period),
                ParseArray(operation.Proposals
                    .Select(x => Base58.Parse(x, 2))
                    .SelectMany(x => x)
                    .ToArray()));
        }

        static byte[] ParseActivation(ActivationContent operation)
        {
            return Concat(
                ParseMicheNat((int)OperationTag.Activation),
                ParseTz1Address(operation.Address),
                Hex.Parse(operation.Secret));
        }

        static byte[] ParseDoubleBaking(DoubleBakingContent operation)
        {
            return Concat(
                ParseMicheNat((int)OperationTag.DoubleBaking),
                ParseArray(ParseBlockHeader(operation.BlockHeader1)),
                ParseArray(ParseBlockHeader(operation.BlockHeader2)));
        }

        static byte[] ParseDoubleEndorsement(DoubleEndorsementContent operation)
        {
            return Concat(
                ParseMicheNat((int)OperationTag.DoubleEndorsement),
                ParseArray(ParseInlinedEndorsement(operation.Op1)),
                ParseArray(ParseInlinedEndorsement(operation.Op2)));
        }

        static byte[] ParseSeedNonceRevelaion(SeedNonceRevelationContent operation)
        {
            return Concat(
                ParseMicheNat((int)OperationTag.SeedNonceRevelation),
                ParseInt32(operation.Level),
                Hex.Parse(operation.Nonce));
        }

        static byte[] ParseDelegation(DelegationContent operation)
        {
            return Concat(
                ParseMicheNat((int)OperationTag.Delegation),
                ParseTzAddress(operation.Source),
                ParseMicheNat(operation.Fee),
                ParseMicheNat(operation.Counter),
                ParseMicheNat(operation.GasLimit),
                ParseMicheNat(operation.StorageLimit),
                ParseDelegate(operation.Delegate));
        }

        static byte[] ParseOrigination(OriginationContent operation)
        {
            return Concat(
                ParseMicheNat((int)OperationTag.Origination),
                ParseTzAddress(operation.Source),
                ParseMicheNat(operation.Fee),
                ParseMicheNat(operation.Counter),
                ParseMicheNat(operation.GasLimit),
                ParseMicheNat(operation.StorageLimit),
                ParseMicheNat(operation.Balance),
                ParseDelegate(operation.Delegate),
                ParseScript(operation.Script));
        }

        static byte[] ParseTransaction(TransactionContent operation)
        {
            return Concat(
                ParseMicheNat((int)OperationTag.Transaction),
                ParseTzAddress(operation.Source),
                ParseMicheNat(operation.Fee),
                ParseMicheNat(operation.Counter),
                ParseMicheNat(operation.GasLimit),
                ParseMicheNat(operation.StorageLimit),
                ParseMicheNat(operation.Amount),
                ParseAddress(operation.Destination),
                ParseParameters(operation.Parameters));
        }

        static byte[] ParseReveal(RevealContent operation)
        {
            return Concat(
                ParseMicheNat((int)OperationTag.Reveal),
                ParseTzAddress(operation.Source),
                ParseMicheNat(operation.Fee),
                ParseMicheNat(operation.Counter),
                ParseMicheNat(operation.GasLimit),
                ParseMicheNat(operation.StorageLimit),
                ParsePublicKey(operation.PublicKey));
        }

        #region nested
        static byte[] ParseBlockHeader(BlockHeader header)
        {
            return Concat(
                ParseInt32(header.Level),
                ParseInt32(header.Proto, 1),
                Base58.Parse(header.Predecessor, 2),
                ParseInt64(header.Timestamp.ToUnixTime()),
                ParseInt32(header.ValidationPass, 1),
                Base58.Parse(header.OperationsHash, 3),
                ParseArray(header.Fitness.Select(x => ParseArray(Hex.Parse(x))).SelectMany(x => x).ToArray()),
                Base58.Parse(header.Context, 2),
                ParseInt32(header.Priority, 2),
                Hex.Parse(header.ProofOfWorkNonce),
                ParseSeedNonce(header.SeedNonceHash),
                Base58.Parse(header.Signature, 3));
        }

        static byte[] ParseInlinedEndorsement(InlinedEndorsement op)
        {
            return Concat(
                Base58.Parse(op.Branch, 2),
                ParseMicheNat((int)OperationTag.Endorsement),
                ParseInt32(op.Operations.Level),
                Base58.Parse(op.Signature, 3));
        }

        static byte[] ParseSeedNonce(string nonce)
        {
            return nonce == null ? ParseBool(false) : Concat(
                ParseBool(true),
                Base58.Parse(nonce, 3));
        }

        static byte[] ParseDelegate(string delegat)
        {
            return delegat == null ? ParseBool(false) : Concat(
                ParseBool(true),
                ParseTzAddress(delegat));
        }

        static byte[] ParseParameters(Parameters param)
        {
            return param == null ? ParseBool(false) : Concat(
                ParseBool(true),
                ParseEntrypoint(param.Entrypoint),
                ParseArray(ParseMicheline(param.Value).ToArray()));
        }

        static byte[] ParseScript(Script script)
        {
            return Concat(
                ParseArray(ParseMicheline(script.Code)),
                ParseArray(ParseMicheline(script.Storage)));
        }
        #endregion
    }
}