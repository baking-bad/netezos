using Netezos.Encoding;
using Netezos.Forging.Models;
using System;
using System.Linq;

namespace Netezos.Forging
{
    public partial class LocalForge
    {
        static OperationContent UnforgeOperation(ForgedReader reader)
        {
            var operation = (int)reader.ReadUBigInt();

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
                case OperationTag.RegisterConstant:
                    return UnforgeRegisterConstant(reader);
                default:
                    throw new ArgumentException($"Invalid operation: {operation}");
            }
        }

        static EndorsementContent UnforgeEndorsement(ForgedReader reader)
        {
            return new EndorsementContent
            {
                Level = reader.ReadInt32()
            };
        }

        static BallotContent UnforgeBallot(ForgedReader reader)
        {
            return new BallotContent
            {
                Source = reader.ReadTzAddress(),
                Period = reader.ReadInt32(),
                Proposal = reader.ReadBase58(Lengths.P.Decoded, Prefix.P),
                Ballot = (Ballot)reader.ReadByte()
            };
        }

        static ProposalsContent UnforgeProposals(ForgedReader reader)
        {
            return new ProposalsContent
            {
                Source = reader.ReadTzAddress(),
                Period = reader.ReadInt32(),
                Proposals = reader.ReadEnumerable(r => r.ReadBase58(Lengths.P.Decoded, Prefix.P)).ToList()
            };
        }

        static ActivationContent UnforgeActivation(ForgedReader reader)
        {
            return new ActivationContent
            {
                Address = reader.ReadTz1Address(),
                Secret = Hex.Convert(reader.ReadBytes(20))
            };
        }

        static DoubleBakingContent UnforgeDoubleBaking(ForgedReader reader)
        {
            return new DoubleBakingContent
            {
                BlockHeader1 = reader.ReadEnumerableSingle(UnforgeBlockHeader),
                BlockHeader2 = reader.ReadEnumerableSingle(UnforgeBlockHeader)
            };
        }

        static DoubleEndorsementContent UnforgeDoubleEndorsement(ForgedReader reader)
        {
            return new DoubleEndorsementContent
            {
                Op1 = reader.ReadEnumerableSingle(UnforgeInlinedEndorsement),
                Op2 = reader.ReadEnumerableSingle(UnforgeInlinedEndorsement),
            };
        }

        static SeedNonceRevelationContent UnforgeSeedNonceRevelaion(ForgedReader reader)
        {
            return new SeedNonceRevelationContent
            {
                Level = reader.ReadInt32(),
                Nonce = Hex.Convert(reader.ReadBytes(Lengths.nce.Decoded))
            };
        }

        static DelegationContent UnforgeDelegation(ForgedReader reader)
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

        static OriginationContent UnforgeOrigination(ForgedReader reader)
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

        static TransactionContent UnforgeTransaction(ForgedReader reader)
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
        
        static RevealContent UnforgeReveal(ForgedReader reader)
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

        static RegisterConstantContent UnforgeRegisterConstant(ForgedReader reader)
        {
            return new RegisterConstantContent
            {
                Source = reader.ReadTzAddress(),
                Fee = (long)reader.ReadUBigInt(),
                Counter = (int)reader.ReadUBigInt(),
                GasLimit = (int)reader.ReadUBigInt(),
                StorageLimit = (int)reader.ReadUBigInt(),
                Value = reader.ReadEnumerableSingle(UnforgeMicheline)
            };
        }

        #region nested

        static BlockHeader UnforgeBlockHeader(ForgedReader reader)
        {
            return new BlockHeader
            {
                Level = reader.ReadInt32(),
                Proto = reader.ReadInt32(1),
                Predecessor = reader.ReadBase58(Lengths.B.Decoded, Prefix.B),
                Timestamp = DateTimeExtension.FromUnixTime(reader.ReadInt64()),
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

        static InlinedEndorsement UnforgeInlinedEndorsement(ForgedReader reader)
        {
            return new InlinedEndorsement
            {
                Branch = reader.ReadBase58(Lengths.B.Decoded, Prefix.B),
                Operations = (EndorsementContent)UnforgeOperation(reader),
                Signature = reader.ReadBase58(Lengths.sig.Decoded, Prefix.sig)
            };
        }

        static string UnforgeSeedNonce(ForgedReader reader)
        {
            return UnforgeConditional(reader, () => reader.ReadBase58(Lengths.nce.Decoded, Prefix.nce));
        }

        static string UnforgeDelegate(ForgedReader reader)
        {
            return UnforgeConditional(reader, () => reader.ReadTzAddress());
        }

        static Parameters UnforgeParameters(ForgedReader reader)
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

        static string UnforgeEntrypoint(ForgedReader reader)
        {
            var ep = reader.ReadInt32(1);

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

        static Script UnforgeScript(ForgedReader reader)
        {
            return new Script
            {
                Code = (MichelineArray)reader.ReadEnumerableSingle(UnforgeMicheline),
                Storage = reader.ReadEnumerableSingle(UnforgeMicheline)
            };
        }

        static T UnforgeConditional<T>(ForgedReader reader, Func<T> tb, Func<T> fb = null)
            where T : class
        {
            return reader.ReadBool() ? tb() : fb?.Invoke();
        }

        #endregion
    }
}
