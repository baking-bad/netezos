using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Netezos.Forge.Operations;
using Netezos.Encoding;
using Netezos.Utils;

namespace Netezos.Forge
{
    public class LocalForge : IForge
    {
        static readonly byte[] BranchPrefix = { 1, 52 };
        static readonly byte[] ProposalPrefix = { 2, 170 };
        static readonly byte[] SigPrefix = { 4, 130, 43 };
        static readonly byte[] OperationPrefix = { 29, 159, 109 };
        static readonly byte[] ContextPrefix = { 79, 179 };
        static readonly Dictionary<string, long> OperationTags = new Dictionary<string, long> {
            { "endorsement", 0 },
            { "proposals", 5 },
            { "ballot", 6 },
            { "seed_nonce_revelation", 1 },
            { "double_endorsement_evidence", 2 },
            { "double_baking_evidence", 3 },
            { "activate_account", 4 },
            { "reveal", 107 },
            { "transaction", 108 },
            { "origination", 109 },
            { "delegation", 110 }
        };

        public Task<byte[]> ForgeOperationAsync(string branch, OperationContent content)
        {
            var res = string.IsNullOrWhiteSpace(branch) ? new byte[]{} : Base58.Parse(branch, BranchPrefix);
            
            return Task.FromResult(res.Concat(ForgeOperation(content)));
        }

        public Task<byte[]> ForgeOperationGroupAsync(string branch, IEnumerable<ManagerOperationContent> contents)
        {
            var res = string.IsNullOrWhiteSpace(branch) ? new byte[]{} : Base58.Parse(branch, BranchPrefix);

            foreach (var operation in contents)
            {
                res = res.Concat(ForgeOperation(operation));
            }
            
            return Task.FromResult(res);
        }

        static byte[] ForgeOperation(OperationContent content)
        {
            switch (content)
            {
                case TransactionContent transaction:
                    return ForgeTransaction(transaction);
                case RevealContent reveal:
                    return ForgeRevelation(reveal);
                case ActivationContent activation:
                    return ForgeActivation(activation);
                case OriginationContent origination:
                    return ForgeOrigination(origination);
                case DelegationContent delegation:
                    return ForgeDelegation(delegation);
                case EndorsementContent endorsement:
                    return ForgeEndorsement(endorsement);
                case SeedNonceRevelationContent seed:
                    return ForgeSeedNonceRevelaion(seed);
                case ProposalsContent proposals:
                    return ForgeProposals(proposals);
                case BallotContent ballot:
                    return ForgeBallot(ballot);
                case DoubleEndorsementEvidenceContent doubleEndorsementEvidence:
                    return ForgeDoubleEndorsementEvidence(doubleEndorsementEvidence);
                case DoubleBakingEvidenceContent doubleBakingEvidence:
                    return ForgeDoubleBakingEvidence(doubleBakingEvidence);
                default:
                    throw new NotImplementedException($"{content.Kind} is not implemented");
            }
        }

        static byte[] ForgeRevelation(RevealContent operation)
        {
            var res = ForgeNat(OperationTags[operation.Kind]);
            res = res.Concat(ForgeSource(operation.Source));
            res = res.Concat(ForgeNat(operation.Fee));
            res = res.Concat(ForgeNat(operation.Counter));
            res = res.Concat(ForgeNat(operation.GasLimit));
            res = res.Concat(ForgeNat(operation.StorageLimit));
            res = res.Concat(ForgePublicKey(operation.PublicKey));

            return res;
        }

        static byte[] ForgeActivation(ActivationContent operation)
        {
            var res = ForgeNat(OperationTags[operation.Kind]);
            res = res.Concat(ForgeActivationAddress(operation.Address));
            res = res.Concat(Hex.Parse(operation.Secret));

            return res;
        }

        static byte[] ForgeTransaction(TransactionContent operation)
        {
            var res = ForgeNat(OperationTags[operation.Kind]);
            res = res.Concat(ForgeSource(operation.Source));
            res = res.Concat(ForgeNat(operation.Fee));
            res = res.Concat(ForgeNat(operation.Counter));
            res = res.Concat(ForgeNat(operation.GasLimit));
            res = res.Concat(ForgeNat(operation.StorageLimit));
            res = res.Concat(ForgeNat(operation.Amount));
            res = res.Concat(ForgeAddress(operation.Destination));

            if (operation.Parameters != null)
            {
                res = res.Concat(ForgeBool(true));
                res = res.Concat(ForgeEntrypoint(operation.Parameters.Entrypoint));
                res = res.Concat(ForgeArray(ForgeMicheline(operation.Parameters.Value).ToArray()));
            }
            else
                res = res.Concat(ForgeBool(false));

            return res;
        }

        static byte[] ForgeOrigination(OriginationContent operation)
        {
            var res = ForgeNat(OperationTags[operation.Kind]);
            res = res.Concat(ForgeSource(operation.Source));
            res = res.Concat(ForgeNat(operation.Fee));
            res = res.Concat(ForgeNat(operation.Counter));
            res = res.Concat(ForgeNat(operation.GasLimit));
            res = res.Concat(ForgeNat(operation.StorageLimit));
            res = res.Concat(ForgeNat(operation.Balance));

            if (!string.IsNullOrWhiteSpace(operation.Delegate))
            {
                res = res.Concat(ForgeBool(true));
                res = res.Concat(ForgeSource(operation.Delegate));
            }
            else
            {
                res = res.Concat(ForgeBool(false));
            }

            res = res.Concat(ForgeScript(operation.Script));
            
            return res;
        }

        static byte[] ForgeDelegation(DelegationContent operation)
        {
            var res = ForgeNat(OperationTags[operation.Kind]);
            res = res.Concat(ForgeSource(operation.Source));
            res = res.Concat(ForgeNat(operation.Fee));
            res = res.Concat(ForgeNat(operation.Counter));
            res = res.Concat(ForgeNat(operation.GasLimit));
            res = res.Concat(ForgeNat(operation.StorageLimit));
            
            if (!string.IsNullOrWhiteSpace(operation.Delegate))
            {
                res = res.Concat(ForgeBool(true));
                res = res.Concat(ForgeSource(operation.Delegate));
            }
            else
            {
                res = res.Concat(ForgeBool(false));
            }

            return res;
        }

        static byte[] ForgeEndorsement(EndorsementContent operation)
        {
            var res = ForgeNat(OperationTags[operation.Kind]);
            res = res.Concat(ForgeInt32(operation.Level));

            return res;
        }

        static byte[] ForgeSeedNonceRevelaion(SeedNonceRevelationContent operation)
        {
            var res = ForgeNat(OperationTags[operation.Kind]);
            res = res.Concat(ForgeInt32(operation.Level));
            res = res.Concat(Hex.Parse(operation.Nonce));

            return res;
        }

        static byte[] ForgeProposals(ProposalsContent operation)
        {
            var res = ForgeNat(OperationTags[operation.Kind]);
            res = res.Concat(ForgeSource(operation.Source));
            res = res.Concat(ForgeInt32(operation.Period));

            var array = new byte[]{};
            foreach (var proposal in operation.Proposals)
            {
                array = array.Concat(Base58.Parse(proposal, ProposalPrefix));
            }
            
            return res.Concat(ForgeArray(array));
        }

        static byte[] ForgeBallot(BallotContent operation)
        {
            var res = ForgeNat(OperationTags[operation.Kind]);
            res = res.Concat(ForgeSource(operation.Source));
            res = res.Concat(ForgeInt32(operation.Period));
            res = res.Concat(Base58.Parse(operation.Proposal, ProposalPrefix));
            res = res.Concat(new[] {(byte) operation.Ballot});
            
            return res;
        }

        static byte[] ForgeDoubleEndorsementEvidence(DoubleEndorsementEvidenceContent operation)
        {
            var res = ForgeNat(OperationTags[operation.Kind]);

            res = res.Concat(ForgeEndorsementOperation(operation.Op1));
            res = res.Concat(ForgeEndorsementOperation(operation.Op2));
            
            return res;
        }

        static byte[] ForgeDoubleBakingEvidence(DoubleBakingEvidenceContent operation)
        {
            var res = ForgeNat(OperationTags[operation.Kind]);

            res = res.Concat(ForgeBlockHeader(operation.BlockHeader1));
            res = res.Concat(ForgeBlockHeader(operation.BlockHeader2));

            return res;
        }

        static byte[] ForgeEndorsementOperation(DoubleEndorsementOperationContent op)
        {
            var op1 = new byte[] { };
            op1 = op1.Concat(Base58.Parse(op.Branch, BranchPrefix));
            op1 = op1.Concat(ForgeNat(OperationTags[op.Operations.Kind]));
            op1 = op1.Concat(ForgeInt32(op.Operations.Level));
            op1 = op1.Concat(Base58.Parse(op.Signature, SigPrefix));
            return ForgeArray(op1);
        }

        static byte[] ForgeBlockHeader(BlockHeader header)
        {
            var bh1 = new byte[] { };
            bh1 = bh1.Concat(ForgeInt32(header.Level));
            bh1 = bh1.Concat(ForgeInt32(header.Proto, 1));
            bh1 = bh1.Concat(Base58.Parse(header.Predecessor, BranchPrefix));
            var timestamp1 = (int)header.Timestamp.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            bh1 = bh1.Concat(ForgeLong(timestamp1));
            bh1 = bh1.Concat(ForgeInt32(header.ValidationPass, 1));
            bh1 = bh1.Concat(Base58.Parse(header.OperationsHash, OperationPrefix));
            var fit1 = new byte[] { };
            foreach (var f in header.Fitness)
            {
                fit1 = fit1.Concat(ForgeArray(Hex.Parse(f)));
            }
            bh1 = bh1.Concat(ForgeArray(fit1));
            bh1 = bh1.Concat(Base58.Parse(header.Context, ContextPrefix));
            bh1 = bh1.Concat(ForgeInt32(header.Priority, 2));
            bh1 = bh1.Concat(Hex.Parse(header.ProofOfWorkNonce));
            bh1 = bh1.Concat(ForgeSign(header.Signature));

            return ForgeArray(bh1);
        }

        static byte[] ForgeScript(Script script)
        {
            var code = ForgeArray(ForgeMicheline(script.Code).ToArray());
            return code.Concat(ForgeArray(ForgeMicheline(script.Storage).ToArray()));
        }

        static byte[] ForgeArray(byte[] value, int len = 4)
        {
            var bytes = BitConverter.GetBytes(value.Length).GetBytes(0, len).Reverse().ToArray();
            return bytes.Concat(value);
        }

        static byte[] ForgeInt32(int value, int len = 4)
        {
            return BitConverter.GetBytes(value).GetBytes(0, len).Reverse().ToArray();
        }

        static byte[] ForgeLong(long value, int len = 8)
        {
            return BitConverter.GetBytes(value).GetBytes(0, len).Reverse().ToArray();
        }

        static byte[] ForgeNat(long value)
        {
            if (value < 0)
                throw new ArgumentException("Value cannot be negative", nameof(value));
            
            var buf = new List<byte>();

            var more = true;

            while (more)
            {
                var b = (byte)(value & 0x7f);        
                value >>= 7;
                if (value > 0)
                    b |= 0x80;
                else
                    more = false;

                buf.Add(b);
            }

            return buf.ToArray();
        }

        static byte[] ForgeSign(string value)
        {
            return new byte[]{0}.Concat(Base58.Parse(value, SigPrefix));
        }
        
        static byte[] ForgeAddress(string value)
        {
            var prefix = value.Substring(0, 3);

            var res = Base58.Parse(value);
            res = res.GetBytes(3, res.Length - 3);

            switch (prefix)
            {
                case "tz1":
                    res = new byte[]{0, 0}.Concat(res);
                    break;
                case "tz2":
                    res = new byte[]{0, 1}.Concat(res);
                    break;
                case "tz3":
                    res = new byte[]{0, 2}.Concat(res);
                    break;
                case "KT1":
                    res = new byte[]{1}.Concat(res).Concat(new byte[]{0});
                    break;
                default:
                    throw new ArgumentException($"Value address exception. Invalid prefix {prefix}");
            }

            return res;
        }
        
        static byte[] ForgeActivationAddress(string value)
        {
            var res = Base58.Parse(value);
            return res.GetBytes(3, res.Length - 3);
        }

        static byte[] ForgeSource(string value)
        {
            var prefix = value.Substring(0, 3);
            
            var res = Base58.Parse(value);
            res = res.GetBytes(3, res.Length - 3);

            switch (prefix)
            {
                case "tz1":
                    res = new byte[]{0}.Concat(res);
                    break;
                case "tz2":
                    res = new byte[]{1}.Concat(res);
                    break;
                case "tz3":
                    res = new byte[]{2}.Concat(res);
                    break;
                default:
                    throw new ArgumentException($"Value address exception. Invalid prefix {prefix}");
            }

            return res;
        }

        static byte[] ForgeBool(bool value)
        {
            return value ? new byte[]{255} : new byte[]{0};
        }

        static byte[] ForgePublicKey(string value)
        {
            var prefix = value.Substring(0, 4);
            var res = Base58.Parse(value);
            res = res.GetBytes(4, res.Length - 4);

            switch (prefix)
            {
                case "edpk":
                    res = new byte[]{0}.Concat(res);
                    break;
                case "sppk":
                    res = new byte[]{1}.Concat(res);
                    break;
                case "p2pk":
                    res = new byte[]{2}.Concat(res);
                    break;
                default:
                    throw new ArgumentException($"Value address exception. Invalid prefix {prefix}");
            }

            return res;
        }
        
        static byte[] ForgeInt(int value)
        {
            var binary = Convert.ToString(Math.Abs(value), 2);

            var pad = 6;
            if ((binary.Length - 6) % 7 == 0)
                pad = binary.Length;
            else if (binary.Length > 6)
                pad = binary.Length + 7 - (binary.Length - 6) % 7;

            binary = binary.PadLeft(pad, '0');

            var septets = new List<string>();

            for (var i = 0; i <= pad / 7; i++)
                septets.Add(binary.Substring(7 * i, Math.Min(7, pad - 7 * i)));

            septets.Reverse();

            septets[0] = (value >= 0 ? "0" : "1") + septets[0];

            var res = new byte[]{};

            for (var i = 0; i < septets.Count; i++)
            {
                var prefix = i == septets.Count - 1 ? "0" : "1";
                res = res.Concat(new []{Convert.ToByte(prefix + septets[i], 2)});
            }

            return res;
        }

        static byte[] ForgeEntrypoint(string value)
        {
            var res = new byte[]{};

            var entrypointTags = new Dictionary<string, byte>
            {
                {"default", 0},
                {"root", 1},
                {"do", 2},
                {"set_delegate", 3},
                {"remove_delegate", 4}
            };
            if (entrypointTags.ContainsKey(value))
            {
                res = res.Concat(new []{entrypointTags[value]});
            }
            else
            {
                res  = res.Concat(new byte[]{255});
                res = res.Concat(ForgeArray(System.Text.Encoding.UTF8.GetBytes(value), 1));
            }

            return res;
        }

        static IEnumerable<byte> ForgeMicheline(IMicheline micheline)
        {
            var res = new List<byte>();

            #region Tags
            var lenTags = new Dictionary<bool, byte>[] {
                new Dictionary<bool, byte> {
                    { false, 3 },
                    { true, 4 }
                },
                new Dictionary<bool, byte> {
                    { false, 5 },
                    { true, 6 }
                },
                new Dictionary<bool, byte> {
                    { false, 7 },
                    { true, 8 }
                },
                new Dictionary<bool, byte> {
                    { false, 9 },
                    { true, 9 }
                }
            };
            #endregion

            switch (micheline)
            {
                case MichelineArray array:
                    res.Add(0x02);
                    res.AddRange(ForgeArray(array.Select(ForgeMicheline).SelectMany(x => x).ToArray()));
                    break;

                case MichelinePrim prim:
                    var argsCnt = prim.Args?.Count ?? 0;
                    var annotsCnt = prim.Annots?.Count ?? 0;

                    res.Add(lenTags[argsCnt][annotsCnt > 0]);
                    res.Add((byte)prim.Prim);

                    if (argsCnt > 0)
                    {
                        var args = prim.Args.Select(ForgeMicheline).SelectMany(x => x);
                        if (argsCnt < 3)
                        {
                            res.AddRange(args);
                        }
                        else
                        {
                            res.AddRange(ForgeArray(args.ToArray()));
                        }
                    }

                    if (annotsCnt > 0)
                    {
                        res.AddRange(ForgeArray(System.Text.Encoding.UTF8.GetBytes(string.Join(" ", prim.Annots))));
                    }

                    else if (argsCnt == 3)
                    {
                        res.AddRange(new List<byte> { 0, 0, 0, 0 });
                    }

                    break;

                case MichelineBytes bytes:
                    res.Add(0x0A);
                    res.AddRange(ForgeArray(bytes.Value));
                    break;

                case MichelineInt @int:
                    res.Add(0x00);
                    res.AddRange(ForgeInt(@int.Value));
                    break;

                case MichelineString str:
                    res.Add(0x01);
                    res.AddRange(ForgeArray(System.Text.Encoding.UTF8.GetBytes(str.Value)));
                    break;
            }

            return res;
        }
    }
}