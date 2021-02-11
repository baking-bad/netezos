using System;
using System.Linq;
using Netezos.Encoding;
using Netezos.Forging.Models;

namespace Netezos.Forging
{
    public partial class LocalForge
    {
        static OperationContent UnforgeOperation(MichelineReader reader)
        {
            int operation = (int)reader.ReadUBigInt();

            switch ((OperationTag)operation)
            {
                case OperationTag.Endorsement:
                    return UnforgeEndorsement(reader);
                case OperationTag.Ballot:
                    return UnforgeBallot(reader);
                case OperationTag.Proposals:
                    return UnforgeProposals(reader);
                case OperationTag.Activation:
                    return UnforgeActivation(reader);
                case OperationTag.DoubleBaking:
                    return UnforgeDoubleBaking(reader);
                case OperationTag.DoubleEndorsement:
                    return UnforgeDoubleEndorsement(reader);
                case OperationTag.SeedNonceRevelation:
                    return UnforgeSeedNonceRevelaion(reader);
                case OperationTag.Delegation:
                    return UnforgeDelegation(reader);
                case OperationTag.Origination:
                    return UnforgeOrigination(reader);
                case OperationTag.Transaction:
                    return UnforgeTransaction(reader);
                case OperationTag.Reveal:
                    return UnforgeReveal(reader);
                default:
                    throw new ArgumentException($"Invalid operation: {operation}");
            }
        }

        static EndorsementContent UnforgeEndorsement(MichelineReader reader)
        {
            return new EndorsementContent
            {
                Level = reader.ReadInt32()
            };
        }

        static BallotContent UnforgeBallot(MichelineReader reader)
        {
            return new BallotContent
            {
                Source = reader.ReadTzAddress(),
                Period = reader.ReadInt32(),
                Proposal = reader.ReadBase58(Lengths.P.Decoded, Prefix.P),
                Ballot = (Ballot)reader.ReadByte()
            };
        }

        static ProposalsContent UnforgeProposals(MichelineReader reader)
        {
            return new ProposalsContent
            {
                Source = reader.ReadTzAddress(),
                Period = reader.ReadInt32(),
                Proposals = reader.ReadEnumerable(r => r.ReadBase58(Lengths.P.Decoded, Prefix.P)).ToList()
            };
        }

        static ActivationContent UnforgeActivation(MichelineReader reader)
        {
            return new ActivationContent
            {
                Address = reader.ReadTz1Address(),
                Secret = Hex.Convert(reader.ReadBytesToEnd())
            };
        }

        static DoubleBakingContent UnforgeDoubleBaking(MichelineReader reader)
        {
            return new DoubleBakingContent
            {
                BlockHeader1 = reader.ReadEnumerableSingle(UnforgeBlockHeader),
                BlockHeader2 = reader.ReadEnumerableSingle(UnforgeBlockHeader)
            };
        }

        static DoubleEndorsementContent UnforgeDoubleEndorsement(MichelineReader reader)
        {
            return new DoubleEndorsementContent
            {
                Op1 = reader.ReadEnumerableSingle(UnforgeInlinedEndorsement),
                Op2 = reader.ReadEnumerableSingle(UnforgeInlinedEndorsement),
            };
        }

        static SeedNonceRevelationContent UnforgeSeedNonceRevelaion(MichelineReader reader)
        {
            return new SeedNonceRevelationContent
            {
                Level = reader.ReadInt32(),
                Nonce = Hex.Convert(reader.ReadBytes(32))
            };
        }

        static DelegationContent UnforgeDelegation(MichelineReader reader)
        {
            return new DelegationContent
            {
                Source = reader.ReadTzAddress(),
                Fee = (long)reader.ReadUBigInt(),
                Counter = (int)reader.ReadUBigInt(),
                GasLimit = (int)reader.ReadUBigInt(),
                StorageLimit = (int)reader.ReadUBigInt(),
                Delegate = UnforgeDelegate(reader)
            };
        }

        static OriginationContent UnforgeOrigination(MichelineReader reader)
        {
            return new OriginationContent
            {
                Source = reader.ReadTzAddress(),
                Fee = (long)reader.ReadUBigInt(),
                Counter = (int)reader.ReadUBigInt(),
                GasLimit = (int)reader.ReadUBigInt(),
                StorageLimit = (int)reader.ReadUBigInt(),
                Balance = (long)reader.ReadUBigInt(),
                Delegate = UnforgeDelegate(reader),
                Script = UnforgeScript(reader)
            };
        }

        static TransactionContent UnforgeTransaction(MichelineReader reader)
        {
            return new TransactionContent
            {
                Source = reader.ReadTzAddress(),
                Fee = (long)reader.ReadUBigInt(),
                Counter = (int)reader.ReadUBigInt(),
                GasLimit = (int)reader.ReadUBigInt(),
                StorageLimit = (int)reader.ReadUBigInt(),
                Amount = (long)reader.ReadUBigInt(),
                Destination = reader.ReadAddress(),
                Parameters = UnforgeParameters(reader)
            };
        }

        static RevealContent UnforgeReveal(MichelineReader reader)
        {
            return new RevealContent
            {
                Source = reader.ReadTzAddress(),
                Fee = (long)reader.ReadUBigInt(),
                Counter = (int)reader.ReadUBigInt(),
                GasLimit = (int)reader.ReadUBigInt(),
                StorageLimit = (int)reader.ReadUBigInt(),
                PublicKey = reader.ReadPublicKey()
            };
        }

        #region nested

        static BlockHeader UnforgeBlockHeader(MichelineReader reader)
        {
            return new BlockHeader
            {
                Level = reader.ReadInt32(),
                Proto = reader.ReadInt32(1),
                Predecessor = reader.ReadBase58(Lengths.B.Decoded, Prefix.B),
                Timestamp = reader.ReadInt64().FromUnixTime(),
                ValidationPass = reader.ReadInt32(1),
                OperationsHash = reader.ReadBase58(Lengths.LLo.Decoded, Prefix.LLo),
                Fitness = reader.ReadEnumerable(r => r.ReadHexString()).ToList(),
                Context = reader.ReadBase58(Lengths.Co.Decoded, Prefix.Co),
                Priority = reader.ReadInt32(2),
                ProofOfWorkNonce = Hex.Convert(reader.ReadBytes(8)),
                SeedNonceHash = UnforgeSeedNonce(reader),
                Signature = reader.ReadBase58(Lengths.sig.Decoded, Prefix.sig),
            };
        }

        static InlinedEndorsement UnforgeInlinedEndorsement(MichelineReader reader)
        {
            return new InlinedEndorsement
            {
                Branch = reader.ReadBase58(Lengths.B.Decoded, Prefix.B),
                Operations = (EndorsementContent)UnforgeOperation(reader),
                Signature = reader.ReadBase58(Lengths.sig.Decoded, Prefix.sig)
            };
        }

        static string UnforgeSeedNonce(MichelineReader reader)
        {
            return UnforgeConditional(reader, () => reader.ReadBase58(Lengths.o.Decoded, Prefix.o)); // TODO: I don't think this is right, but can't find an example.
        }

        static string UnforgeDelegate(MichelineReader reader)
        {
            return UnforgeConditional(reader, () => reader.ReadTzAddress());
        }

        static Parameters UnforgeParameters(MichelineReader reader)
        {
            return UnforgeConditional(reader, () =>
            {
                return new Parameters
                {
                    Entrypoint = UnforgeEntrypoint(reader),
                    Value = reader.ReadEnumerableSingle(UnforgeMicheline)
                };
            });
        }

        static string UnforgeEntrypoint(MichelineReader reader)
        {
            int ep = reader.ReadInt32(1);

            switch (ep)
            {
                case 0: return "default";
                case 1: return "root";
                case 2: return "do";
                case 3: return "set_delegate";
                case 4: return "remove_delegate";
                case 255: return reader.ReadString(1);
                default: throw new ArgumentException($"Unrecognized endpoint type {ep}");
            }
        }

        static Script UnforgeScript(MichelineReader reader)
        {
            return new Script
            {
                Code = (MichelineArray)reader.ReadEnumerableSingle(UnforgeMicheline),
                Storage = reader.ReadEnumerableSingle(UnforgeMicheline)
            };
        }

        static T UnforgeConditional<T>(MichelineReader reader, Func<T> tb, Func<T> fb = null)
            where T : class
        {
            return reader.ReadBool() ? tb() : fb?.Invoke();
        }

        #endregion
    }
}
