﻿using Netezos.Encoding;
using Netezos.Forging.Models;

namespace Netezos.Forging
{
    public partial class LocalForge
    {
        static OperationContent UnforgeOperation(ForgedReader reader)
        {
            return (OperationTag)reader.ReadByte() switch
            {
                OperationTag.Attestation => UnforgeAttestation(reader),
                OperationTag.AttestationWithDal => UnforgeAttestationWithDal(reader),
                OperationTag.Preattestation => UnforgePreattestation(reader),
                OperationTag.Ballot => UnforgeBallot(reader),
                OperationTag.Proposals => UnforgeProposals(reader),
                OperationTag.Activation => UnforgeActivation(reader),
                OperationTag.DoubleBaking => UnforgeDoubleBaking(reader),
                OperationTag.DoubleAttestation => UnforgeDoubleAttestation(reader),
                OperationTag.DoublePreattestation => UnforgeDoublePreattestation(reader),
                OperationTag.SeedNonceRevelation => UnforgeSeedNonceRevelation(reader),
                OperationTag.VdfRevelation => UnforgeVdfRevelation(reader),
                OperationTag.DrainDelegate => UnforgeDrainDelegate(reader),
                OperationTag.Delegation => UnforgeDelegation(reader),
                OperationTag.Origination => UnforgeOrigination(reader),
                OperationTag.Transaction => UnforgeTransaction(reader),
                OperationTag.Reveal => UnforgeReveal(reader),
                OperationTag.RegisterConstant => UnforgeRegisterConstant(reader),
                OperationTag.SetDepositsLimit => UnforgeSetDepositsLimit(reader),
                OperationTag.IncreasePaidStorage => UnforgeIncreasePaidStorage(reader),
                OperationTag.FailingNoop => UnforgeFailingNoop(reader),
                OperationTag.TransferTicket => UnforgeTransferTicket(reader),
                OperationTag.TxRollupCommit => UnforgeTxRollupCommit(reader),
                OperationTag.TxRollupDispatchTickets => UnforgeTxRollupDispatchTickets(reader),
                OperationTag.TxRollupFinalizeCommitment => UnforgeTxRollupFinalizeCommitment(reader),
                OperationTag.TxRollupOrigination => UnforgeTxRollupOrigination(reader),
                OperationTag.TxRollupRejection => UnforgeTxRollupRejection(reader),
                OperationTag.TxRollupRemoveCommitment => UnforgeTxRollupRemoveCommitment(reader),
                OperationTag.TxRollupReturnBond => UnforgeTxRollupReturnBond(reader),
                OperationTag.TxRollupSubmitBatch => UnforgeTxRollupSubmitBatch(reader),
                OperationTag.UpdateConsensusKey => UnforgeUpdateConsensusKey(reader),
                OperationTag.SrAddMessages => UnforgeSrAddMessages(reader),
                OperationTag.SrCement => UnforgeSrCement(reader),
                OperationTag.SrTimeout => UnforgeSrTimeout(reader),
                OperationTag.SrExecute => UnforgeSrExecute(reader),
                OperationTag.SrOriginate => UnforgeSrOriginate(reader),
                OperationTag.SrPublish => UnforgeSrPublish(reader),
                OperationTag.SrRecoverBond => UnforgeSrRecoverBond(reader),
                OperationTag.SrRefute => UnforgeSrRefute(reader),
                OperationTag.DalPublishCommitment => UnforgeDalPublishCommitment(reader),
                OperationTag.DalEntrapmentEvidence => UnforgeDalEntrapmentEvidence(reader),
                var operation => throw new ArgumentException($"Invalid operation: {operation}")
            };
        }

        static AttestationContent UnforgeAttestation(ForgedReader reader)
        {
            return new AttestationContent
            {
                Slot = reader.ReadInt32(2),
                Level = reader.ReadInt32(),
                Round = reader.ReadInt32(),
                PayloadHash = reader.ReadBase58(32, Prefix.vh),
                DalAttestation = null
            };
        }

        static AttestationContent UnforgeAttestationWithDal(ForgedReader reader)
        {
            return new AttestationContent
            {
                Slot = reader.ReadInt32(2),
                Level = reader.ReadInt32(),
                Round = reader.ReadInt32(),
                PayloadHash = reader.ReadBase58(32, Prefix.vh),
                DalAttestation = reader.ReadMichelineInt().Value
            };
        }

        static PreattestationContent UnforgePreattestation(ForgedReader reader)
        {
            return new PreattestationContent
            {
                Slot = reader.ReadInt32(2),
                Level = reader.ReadInt32(),
                Round = reader.ReadInt32(),
                PayloadHash = reader.ReadBase58(32, Prefix.vh)
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

        static DoubleAttestationContent UnforgeDoubleAttestation(ForgedReader reader)
        {
            return new DoubleAttestationContent
            {
                Op1 = reader.ReadEnumerableSingle(UnforgeInlineAttestation),
                Op2 = reader.ReadEnumerableSingle(UnforgeInlineAttestation)
            };
        }

        static DoublePreattestationContent UnforgeDoublePreattestation(ForgedReader reader)
        {
            return new DoublePreattestationContent
            {
                Op1 = reader.ReadEnumerableSingle(UnforgeInlinePreattestation),
                Op2 = reader.ReadEnumerableSingle(UnforgeInlinePreattestation)
            };
        }

        static SeedNonceRevelationContent UnforgeSeedNonceRevelation(ForgedReader reader)
        {
            return new SeedNonceRevelationContent
            {
                Level = reader.ReadInt32(),
                Nonce = Hex.Convert(reader.ReadBytes(Lengths.nce.Decoded))
            };
        }

        static VdfRevelationContent UnforgeVdfRevelation(ForgedReader reader)
        {
            return new VdfRevelationContent
            {
                Solution = new(2)
                {
                    Hex.Convert(reader.ReadBytes(100)),
                    Hex.Convert(reader.ReadBytes(100))
                }
            };
        }

        static DrainDelegateContent UnforgeDrainDelegate(ForgedReader reader)
        {
            return new DrainDelegateContent
            {
                ConsensusKey = reader.ReadTzAddress(),
                Delegate = reader.ReadTzAddress(),
                Destination = reader.ReadTzAddress()
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

        static SetDepositsLimitContent UnforgeSetDepositsLimit(ForgedReader reader)
        {
            return new SetDepositsLimitContent
            {
                Source = reader.ReadTzAddress(),
                Fee = (long)reader.ReadUBigInt(),
                Counter = (int)reader.ReadUBigInt(),
                GasLimit = (int)reader.ReadUBigInt(),
                StorageLimit = (int)reader.ReadUBigInt(),
                Limit = reader.ReadBool() ? reader.ReadUBigInt() : null
            };
        }

        static IncreasePaidStorageContent UnforgeIncreasePaidStorage(ForgedReader reader)
        {
            return new IncreasePaidStorageContent
            {
                Source = reader.ReadTzAddress(),
                Fee = (long)reader.ReadUBigInt(),
                Counter = (int)reader.ReadUBigInt(),
                GasLimit = (int)reader.ReadUBigInt(),
                StorageLimit = (int)reader.ReadUBigInt(),
                Amount = reader.ReadMichelineInt().Value,
                Destination = reader.ReadAddress()
            };
        }

        static FailingNoopContent UnforgeFailingNoop(ForgedReader reader)
        {
            return new FailingNoopContent
            {
                Message = Utf8.Convert(reader.ReadArray())
            };
        }

        static TransferTicketContent UnforgeTransferTicket(ForgedReader reader)
        {
            return new TransferTicketContent
            {
                Source = reader.ReadTzAddress(),
                Fee = (long)reader.ReadUBigInt(),
                Counter = (int)reader.ReadUBigInt(),
                GasLimit = (int)reader.ReadUBigInt(),
                StorageLimit = (int)reader.ReadUBigInt(),
                TicketContent = reader.ReadEnumerableSingle(UnforgeMicheline),
                TicketType = reader.ReadEnumerableSingle(UnforgeMicheline),
                TicketTicketer = reader.ReadAddress(),
                TicketAmount = reader.ReadUBigInt(),
                Destination = reader.ReadAddress(),
                Entrypoint = reader.ReadString()
            };
        }

        static TxRollupCommitContent UnforgeTxRollupCommit(ForgedReader reader)
        {
            return new TxRollupCommitContent
            {
                Source = reader.ReadTzAddress(),
                Fee = (long)reader.ReadUBigInt(),
                Counter = (int)reader.ReadUBigInt(),
                GasLimit = (int)reader.ReadUBigInt(),
                StorageLimit = (int)reader.ReadUBigInt(),
                Rollup = Base58.Convert(reader.ReadBytes(20), Prefix.txr1),
                Commitment = new TxRollupCommitment
                {
                    Level = reader.ReadInt32(),
                    Messages = reader.ReadEnumerable(r => r.ReadBase58(32, Prefix.txmr)).ToList(),
                    Predecessor = reader.ReadByte() == 1
                        ? Base58.Convert(reader.ReadBytes(32), Prefix.txc)
                        : null,
                    InboxMerkleRoot = Base58.Convert(reader.ReadBytes(32), Prefix.txi)
                }
            };
        }

        static TxRollupDispatchTicketsContent UnforgeTxRollupDispatchTickets(ForgedReader reader)
        {
            return new TxRollupDispatchTicketsContent
            {
                Source = reader.ReadTzAddress(),
                Fee = (long)reader.ReadUBigInt(),
                Counter = (int)reader.ReadUBigInt(),
                GasLimit = (int)reader.ReadUBigInt(),
                StorageLimit = (int)reader.ReadUBigInt(),
                Rollup = Base58.Convert(reader.ReadBytes(20), Prefix.txr1),
                Level = reader.ReadInt32(),
                ContextHash = Base58.Convert(reader.ReadBytes(32), Prefix.Co),
                MessageIndex = reader.ReadInt32(),
                MessageResultPath = reader.ReadEnumerable(r => r.ReadBase58(32, Prefix.txM)).ToList(),
                TicketsInfo = reader.ReadEnumerable(r => new TxRollupTicketsInfo
                {
                    Contents = r.ReadEnumerableSingle(UnforgeMicheline),
                    Type = r.ReadEnumerableSingle(UnforgeMicheline),
                    Ticketer = r.ReadAddress(),
                    Amount = r.ReadInt64(1 << r.ReadByte()),
                    Claimer = r.ReadTzAddress()
                }).ToList()
            };
        }

        static TxRollupFinalizeCommitmentContent UnforgeTxRollupFinalizeCommitment(ForgedReader reader)
        {
            return new TxRollupFinalizeCommitmentContent
            {
                Source = reader.ReadTzAddress(),
                Fee = (long)reader.ReadUBigInt(),
                Counter = (int)reader.ReadUBigInt(),
                GasLimit = (int)reader.ReadUBigInt(),
                StorageLimit = (int)reader.ReadUBigInt(),
                Rollup = Base58.Convert(reader.ReadBytes(20), Prefix.txr1)
            };
        }

        static TxRollupOriginationContent UnforgeTxRollupOrigination(ForgedReader reader)
        {
            return new TxRollupOriginationContent
            {
                Source = reader.ReadTzAddress(),
                Fee = (long)reader.ReadUBigInt(),
                Counter = (int)reader.ReadUBigInt(),
                GasLimit = (int)reader.ReadUBigInt(),
                StorageLimit = (int)reader.ReadUBigInt()
            };
        }

        static TxRollupRejectionContent UnforgeTxRollupRejection(ForgedReader reader)
        {
            throw new NotImplementedException("Due to lack of examples no one knows how to handle it");
        }

        static TxRollupRemoveCommitmentContent UnforgeTxRollupRemoveCommitment(ForgedReader reader)
        {
            return new TxRollupRemoveCommitmentContent
            {
                Source = reader.ReadTzAddress(),
                Fee = (long)reader.ReadUBigInt(),
                Counter = (int)reader.ReadUBigInt(),
                GasLimit = (int)reader.ReadUBigInt(),
                StorageLimit = (int)reader.ReadUBigInt(),
                Rollup = Base58.Convert(reader.ReadBytes(20), Prefix.txr1)
            };
        }

        static TxRollupReturnBondContent UnforgeTxRollupReturnBond(ForgedReader reader)
        {
            return new TxRollupReturnBondContent
            {
                Source = reader.ReadTzAddress(),
                Fee = (long)reader.ReadUBigInt(),
                Counter = (int)reader.ReadUBigInt(),
                GasLimit = (int)reader.ReadUBigInt(),
                StorageLimit = (int)reader.ReadUBigInt(),
                Rollup = Base58.Convert(reader.ReadBytes(20), Prefix.txr1)
            };
        }

        static TxRollupSubmitBatchContent UnforgeTxRollupSubmitBatch(ForgedReader reader)
        {
            return new TxRollupSubmitBatchContent
            {
                Source = reader.ReadTzAddress(),
                Fee = (long)reader.ReadUBigInt(),
                Counter = (int)reader.ReadUBigInt(),
                GasLimit = (int)reader.ReadUBigInt(),
                StorageLimit = (int)reader.ReadUBigInt(),
                Rollup = Base58.Convert(reader.ReadBytes(20), Prefix.txr1),
                Content = reader.ReadArray(),
                BurnLimit = reader.ReadBool() ? (long)reader.ReadUBigInt() : null
            };
        }

        static UpdateConsensusKeyContent UnforgeUpdateConsensusKey(ForgedReader reader)
        {
            return new UpdateConsensusKeyContent
            {
                Source = reader.ReadTzAddress(),
                Fee = (long)reader.ReadUBigInt(),
                Counter = (int)reader.ReadUBigInt(),
                GasLimit = (int)reader.ReadUBigInt(),
                StorageLimit = (int)reader.ReadUBigInt(),
                PublicKey = reader.ReadPublicKey(),
                Proof = reader.ReadBool() ? reader.ReadBlsig() : null
            };
        }

        static SrAddMessagesContent UnforgeSrAddMessages(ForgedReader reader)
        {
            return new SrAddMessagesContent
            {
                Source = reader.ReadTzAddress(),
                Fee = (long)reader.ReadUBigInt(),
                Counter = (int)reader.ReadUBigInt(),
                GasLimit = (int)reader.ReadUBigInt(),
                StorageLimit = (int)reader.ReadUBigInt(),
                Messages = reader.ReadEnumerable(r => r.ReadArray()).ToList(),

            };
        }

        static SrCementContent UnforgeSrCement(ForgedReader reader)
        {
            return new SrCementContent
            {
                Source = reader.ReadTzAddress(),
                Fee = (long)reader.ReadUBigInt(),
                Counter = (int)reader.ReadUBigInt(),
                GasLimit = (int)reader.ReadUBigInt(),
                StorageLimit = (int)reader.ReadUBigInt(),
                Rollup = reader.ReadRollup()
            };
        }

        static SrTmieoutContent UnforgeSrTimeout(ForgedReader reader)
        {
            return new SrTmieoutContent
            {
                Source = reader.ReadTzAddress(),
                Fee = (long)reader.ReadUBigInt(),
                Counter = (int)reader.ReadUBigInt(),
                GasLimit = (int)reader.ReadUBigInt(),
                StorageLimit = (int)reader.ReadUBigInt(),
                Rollup = reader.ReadRollup(),
                Stakers = new StakersPair
                {
                    Alice = reader.ReadTzAddress(),
                    Bob = reader.ReadTzAddress()
                }
            };
        }

        static SrExecuteContent UnforgeSrExecute(ForgedReader reader)
        {
            return new SrExecuteContent
            {
                Source = reader.ReadTzAddress(),
                Fee = (long)reader.ReadUBigInt(),
                Counter = (int)reader.ReadUBigInt(),
                GasLimit = (int)reader.ReadUBigInt(),
                StorageLimit = (int)reader.ReadUBigInt(),
                Rollup = reader.ReadRollup(),
                Commitment = reader.ReadCommitmentAddress(),
                OutputProof = reader.ReadArray()
            };
        }

        static SrOriginateContent UnforgeSrOriginate(ForgedReader reader)
        {
            return new SrOriginateContent
            {
                Source = reader.ReadTzAddress(),
                Fee = (long)reader.ReadUBigInt(),
                Counter = (int)reader.ReadUBigInt(),
                GasLimit = (int)reader.ReadUBigInt(),
                StorageLimit = (int)reader.ReadUBigInt(),
                PvmKind = (PvmKind)reader.ReadByte(),
                Kernel = reader.ReadArray(),
                OriginationProof = reader.ReadArray(),
                ParametersType = reader.ReadEnumerableSingle(UnforgeMicheline)
            };
        }

        static SrPublishContent UnforgeSrPublish(ForgedReader reader)
        {
            return new SrPublishContent
            {
                Source = reader.ReadTzAddress(),
                Fee = (long)reader.ReadUBigInt(),
                Counter = (int)reader.ReadUBigInt(),
                GasLimit = (int)reader.ReadUBigInt(),
                StorageLimit = (int)reader.ReadUBigInt(),
                Rollup = reader.ReadRollup(),
                Commitment = UnforgeCommitment(reader)
            };
        }

        static SrRecoverBondContent UnforgeSrRecoverBond(ForgedReader reader)
        {
            return new SrRecoverBondContent
            {
                Source = reader.ReadTzAddress(),
                Fee = (long)reader.ReadUBigInt(),
                Counter = (int)reader.ReadUBigInt(),
                GasLimit = (int)reader.ReadUBigInt(),
                StorageLimit = (int)reader.ReadUBigInt(),
                Rollup = reader.ReadRollup(),
                Staker = reader.ReadTzAddress()
            };
        }

        static SrRefuteContent UnforgeSrRefute(ForgedReader reader)
        {
            return new SrRefuteContent
            {
                Source = reader.ReadTzAddress(),
                Fee = (long)reader.ReadUBigInt(),
                Counter = (int)reader.ReadUBigInt(),
                GasLimit = (int)reader.ReadUBigInt(),
                StorageLimit = (int)reader.ReadUBigInt(),
                Rollup = reader.ReadRollup(),
                Opponent = reader.ReadTzAddress(),
                Refutation = UnforgeRefutation(reader)
            };
        }

        static DalPublishCommitmentContent UnforgeDalPublishCommitment(ForgedReader reader)
        {
            return new DalPublishCommitmentContent
            {
                Source = reader.ReadTzAddress(),
                Fee = (long)reader.ReadUBigInt(),
                Counter = (int)reader.ReadUBigInt(),
                GasLimit = (int)reader.ReadUBigInt(),
                StorageLimit = (int)reader.ReadUBigInt(),
                SlotHeader = new DalSlotHeader
                {
                    SlotIndex = reader.ReadByte(),
                    Commitment = reader.ReadBase58(48, Prefix.sh),
                    CommitmentProof = reader.ReadBytes(96)
                }
            };
        }

        static DalEntrapmentEvidenceContent UnforgeDalEntrapmentEvidence(ForgedReader reader)
        {
            return new DalEntrapmentEvidenceContent
            {
                Attestation = reader.ReadEnumerableSingle(UnforgeInlineAttestation),
                SlotIndex = reader.ReadByte(),
                ShardWithProof = new ShardWithProof()
                {
                    Shard = new ShardData()
                    {
                        Id = reader.ReadInt32(),
                        Hashes = reader.ReadShardHashes().ToList()
                    },
                    Proof = reader.ReadBase58(48, Prefix.sh)
                }
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
                PayloadHash = reader.ReadBase58(Lengths.vh.Decoded, Prefix.vh),
                PayloadRound = reader.ReadInt32(2),
                ProofOfWorkNonce = Hex.Convert(reader.ReadBytes(8)),
                SeedNonceHash = UnforgeSeedNonce(reader),
                LiquidityBakingToggleVote = (LBToggle)reader.ReadByte(),
                Signature = reader.ReadBase58(Lengths.sig.Decoded, Prefix.sig),
            };
        }

        static InlineAttestation UnforgeInlineAttestation(ForgedReader reader)
        {
            return new InlineAttestation
            {
                Branch = reader.ReadBase58(Lengths.B.Decoded, Prefix.B),
                Operations = (AttestationContent)UnforgeOperation(reader),
                Signature = reader.ReadBase58(Lengths.sig.Decoded, Prefix.sig)
            };
        }

        static InlinePreattestation UnforgeInlinePreattestation(ForgedReader reader)
        {
            return new InlinePreattestation
            {
                Branch = reader.ReadBase58(Lengths.B.Decoded, Prefix.B),
                Operations = (PreattestationContent)UnforgeOperation(reader),
                Signature = reader.ReadBase58(Lengths.sig.Decoded, Prefix.sig)
            };
        }

        static string? UnforgeSeedNonce(ForgedReader reader)
        {
            return UnforgeConditional(reader, () => reader.ReadBase58(Lengths.nce.Decoded, Prefix.nce));
        }

        static string? UnforgeDelegate(ForgedReader reader)
        {
            return UnforgeConditional(reader, () => reader.ReadTzAddress());
        }

        static Parameters? UnforgeParameters(ForgedReader reader)
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
            return reader.ReadInt32(1) switch
            {
                0 => "default",
                1 => "root",
                2 => "do",
                3 => "set_delegate",
                4 => "remove_delegate",
                5 => "deposit",
                6 => "stake",
                7 => "unstake",
                8 => "finalize_unstake",
                9 => "set_delegate_parameters",
                255 => reader.ReadString(1),
                var ep => throw new ArgumentException($"Unrecognized endpoint type {ep}")
            };
        }

        static Script UnforgeScript(ForgedReader reader)
        {
            return new Script
            {
                Code = (MichelineArray)reader.ReadEnumerableSingle(UnforgeMicheline),
                Storage = reader.ReadEnumerableSingle(UnforgeMicheline)
            };
        }

        static Commitment UnforgeCommitment(ForgedReader reader)
        {
            return new Commitment
            {
                State = reader.ReadBase58(Lengths.srs1.Decoded, Prefix.srs1),
                InboxLevel = reader.ReadInt32(),
                Predecessor = reader.ReadBase58(Lengths.src1.Decoded, Prefix.src1),
                Ticks = reader.ReadInt64()
            };
        }

        static RefutationMove UnforgeRefutation(ForgedReader reader)
        {
            switch (reader.ReadByte())
            {
                case 0:
                    return new RefutationStart
                    {
                        PlayerCommitment = reader.ReadCommitmentAddress(),
                        OpponentCommitment = reader.ReadCommitmentAddress()
                    };
                case 1:
                    var choice = (long)reader.ReadUBigInt();
                    return reader.ReadByte() switch
                    {
                        0 => new RefutationDissection
                        {
                            Choice = choice,
                            Steps = reader.ReadEnumerable(UnforgeDissection).ToList(),
                        },
                        1 => new RefutationProof
                        {
                            Choice = choice,
                            Step = UnforgeProofStep(reader)
                        },
                        var ep => throw new ArgumentException($"Unrecognized refutation move step {ep}")
                    };
                default:
                    throw new ArgumentException("Unrecognized refutation type");
            };
        }

        static DissectionStep UnforgeDissection(ForgedReader reader)
        {
            return new DissectionStep
            {
                State = UnforgeConditional(reader, () => reader.ReadBase58(Lengths.src1.Decoded, Prefix.srs1)),
                Tick = (long)reader.ReadUBigInt()
            };
        }

        static ProofStep UnforgeProofStep(ForgedReader reader)
        {
            return new ProofStep
            {
                PvmStep = reader.ReadArray(),
                InputProof = UnforgeConditional(reader, () => UnforgeInputProof(reader))
            };
        }

        static InputProof UnforgeInputProof(ForgedReader reader)
        {
            return reader.ReadByte() switch
            {
                0 => new InboxProof
                {
                    Level = reader.ReadInt32(),
                    MessageCounter = (long)reader.ReadUBigInt(),
                    Proof = reader.ReadArray()
                },
                1 => UnforgeRevealProof(reader),
                2 => new FirstInputProof(),
                _ => throw new ArgumentException("Invalid input proof type")
            };
        }

        static RevealProof UnforgeRevealProof(ForgedReader reader)
        {
            return new RevealProof
            {
                Proof = reader.ReadInt32(1) switch
                {
                    0 => new RawDataProof
                    {
                        RawData = reader.ReadBytes(reader.ReadInt32(2)),
                    },
                    1 => new MetadataProof(),
                    2 => new DalPageProof
                    {
                        DalPageId = new DalPageId
                        {
                            PublishedLevel = reader.ReadInt32(4),
                            SlotIndex = reader.ReadInt32(1),
                            PageIndex = reader.ReadInt32(2)
                        },
                        Proof = reader.ReadArray()
                    },
                    _ => throw new ArgumentException("Invalid reveal proof type")
                }
            };
        }

        static T? UnforgeConditional<T>(ForgedReader reader, Func<T> tb, Func<T>? fb = null)
            where T : class
        {
            return reader.ReadBool() ? tb() : fb?.Invoke();
        }
        #endregion
    }
}
