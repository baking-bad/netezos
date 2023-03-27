using Netezos.Encoding;
using Netezos.Forging.Models;

namespace Netezos.Forging
{
    public partial class LocalForge
    {
        static OperationContent UnforgeOperation(ForgedReader reader)
        {
            return (OperationTag)reader.ReadByte() switch
            {
                OperationTag.Endorsement => UnforgeEndorsement(reader),
                OperationTag.Preendorsement => UnforgePreendorsement(reader),
                OperationTag.Ballot => UnforgeBallot(reader),
                OperationTag.Proposals => UnforgeProposals(reader),
                OperationTag.Activation => UnforgeActivation(reader),
                OperationTag.DoubleBaking => UnforgeDoubleBaking(reader),
                OperationTag.DoubleEndorsement => UnforgeDoubleEndorsement(reader),
                OperationTag.DoublePreendorsement => UnforgeDoublePreendorsement(reader),
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
                OperationTag.SrOriginate=> UnforgeSrOriginate(reader),
                OperationTag.SrPublish=> UnforgeSrPublish(reader),
                OperationTag.SrRecoverBond=> UnforgeSrRecoverBond(reader),
                OperationTag.SrRefute => UnforgeSrRefute(reader),
                var operation => throw new ArgumentException($"Invalid operation: {operation}")
            };
        }

        static EndorsementContent UnforgeEndorsement(ForgedReader reader)
        {
            return new EndorsementContent
            {
                Slot = reader.ReadInt32(2),
                Level = reader.ReadInt32(),
                Round = reader.ReadInt32(),
                PayloadHash = reader.ReadBase58(32, Prefix.vh)
            };
        }

        static PreendorsementContent UnforgePreendorsement(ForgedReader reader)
        {
            return new PreendorsementContent
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

        static DoubleEndorsementContent UnforgeDoubleEndorsement(ForgedReader reader)
        {
            return new DoubleEndorsementContent
            {
                Op1 = reader.ReadEnumerableSingle(UnforgeInlineEndorsement),
                Op2 = reader.ReadEnumerableSingle(UnforgeInlineEndorsement)
            };
        }

        static DoublePreendorsementContent UnforgeDoublePreendorsement(ForgedReader reader)
        {
            return new DoublePreendorsementContent
            {
                Op1 = reader.ReadEnumerableSingle(UnforgeInlinePreendorsement),
                Op2 = reader.ReadEnumerableSingle(UnforgeInlinePreendorsement)
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
                PublicKey = reader.ReadPublicKey()
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
                Message = reader.ReadEnumerable(r => r.ReadArray()).ToList(),

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
                Rollup = reader.ReadSrAddress(),
                Commitment = reader.ReadCommitmentAddress()
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
                Rollup = reader.ReadSrAddress(),
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
                Rollup = reader.ReadSrAddress(),
                CementedCommitment = reader.ReadCommitmentAddress(),
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
                ParametersTy = reader.ReadEnumerableSingle(UnforgeMicheline)
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
                Rollup = reader.ReadSrAddress(),
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
                Rollup = reader.ReadSrAddress(),
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
                Rollup = reader.ReadSrAddress(),
                Opponent = reader.ReadTzAddress(),
                Refutation = UnforgeRefutation(reader)
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

        static InlineEndorsement UnforgeInlineEndorsement(ForgedReader reader)
        {
            return new InlineEndorsement
            {
                Branch = reader.ReadBase58(Lengths.B.Decoded, Prefix.B),
                Operations = (EndorsementContent)UnforgeOperation(reader),
                Signature = reader.ReadBase58(Lengths.sig.Decoded, Prefix.sig)
            };
        }

        static InlinePreendorsement UnforgeInlinePreendorsement(ForgedReader reader)
        {
            return new InlinePreendorsement
            {
                Branch = reader.ReadBase58(Lengths.B.Decoded, Prefix.B),
                Operations = (PreendorsementContent)UnforgeOperation(reader),
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
                CompressedState = reader.ReadBase58(Lengths.srs1.Decoded, Prefix.srs1),
                InboxLevel = reader.ReadInt32(),
                Predecessor = reader.ReadBase58(Lengths.src1.Decoded, Prefix.src1),
                NumberOfTicks = reader.ReadInt64()
            };
        }

        static Refutation UnforgeRefutation(ForgedReader reader)
        {
            switch (reader.ReadByte())
            {
                case 0:
                    return new RefutationStart
                    {
                        PlayerCommitmentHash = reader.ReadCommitmentAddress(),
                        OpponentCommitmentHash = reader.ReadCommitmentAddress()
                    };
                case 1:
                    var choice = (long)reader.ReadUBigInt();
                    return reader.ReadByte() switch
                    {
                        0 => new RefutationDissectionMove
                        {
                            Choice = choice,
                            Step = reader.ReadEnumerable(UnforgeDissection).ToList(),
                        },
                        1 => new RefutationProofMove
                        {
                            Choice = choice,
                            Step = UnforgeProof(reader)
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

        static ProofStep UnforgeProof(ForgedReader reader)
        {
            return new ProofStep
            {
                PvmStep = reader.ReadArray(),
                InputProof = UnforgeConditional<InputProof>(reader, () =>
                {
                    return reader.ReadByte() switch
                    {
                        0 => new InboxProof
                        {
                            Level = reader.ReadInt32(),
                            MessageCounter = (long)reader.ReadUBigInt(),
                            SerializedProof = reader.ReadArray()
                        },
                        1 => new RevealProof
                        {
                            RevealProofData = reader.ReadInt32(1) switch
                            {
                                0 => new RawDataProof
                                {
                                    RawData = reader.ReadBytes(reader.ReadInt32(2)),
                                },
                                 1 => new MetadataProof(),
                                2 => new DalPageProof
                                {
                                    DalProof = reader.ReadArray()
                                }
                            }
                        },

                    };
                })
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
