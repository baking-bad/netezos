using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Netezos.Forge.Operations;
using Netezos.Forge.Utils;
using Newtonsoft.Json.Linq;

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
//                Console.WriteLine($"Res with entrypoint {Hex.Convert(res)}");
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
//            Console.WriteLine($"Array bytes len {Hex.Convert(bytes)}");
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
                res = res.Concat(ForgeArray(Encoding.UTF8.GetBytes(value), 1));
            }

            return res;
        }

        static IEnumerable<byte> ForgeMicheline(JToken data)
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
            
            var primTags = new Dictionary<string, byte> {
                {"parameter", 0x00 },
                {"storage", 0x01 },
                {"code", 0x02 },
                {"False", 0x03 },
                {"Elt", 0x04 },
                {"Left", 0x05 },
                {"None", 0x06 },
                {"Pair", 0x07 },
                {"Right", 0x08 },
                {"Some", 0x09 },
                {"True", 0x0A },
                {"Unit", 0x0B },
                {"PACK", 0x0C },
                {"UNPACK", 0x0D },
                {"BLAKE2B", 0x0E },
                {"SHA256", 0x0F },
                {"SHA512", 0x10 },
                {"ABS", 0x11 },
                {"ADD", 0x12 },
                {"AMOUNT", 0x13 },
                {"AND", 0x14 },
                {"BALANCE", 0x15 },
                {"CAR", 0x16 },
                {"CDR", 0x17 },
                {"CHECK_SIGNATURE", 0x18 },
                {"COMPARE", 0x19 },
                {"CONCAT", 0x1A },
                {"CONS", 0x1B },
                {"CREATE_ACCOUNT", 0x1C },
                {"CREATE_CONTRACT", 0x1D },
                {"IMPLICIT_ACCOUNT", 0x1E },
                {"DIP", 0x1F },
                {"DROP", 0x20 },
                {"DUP", 0x21 },
                {"EDIV", 0x22 },
                {"EMPTY_MAP", 0x23 },
                {"EMPTY_SET", 0x24 },
                {"EQ", 0x25 },
                {"EXEC", 0x26 },
                {"FAILWITH", 0x27 },
                {"GE", 0x28 },
                {"GET", 0x29 },
                {"GT", 0x2A },
                {"HASH_KEY", 0x2B },
                {"IF", 0x2C },
                {"IF_CONS", 0x2D },
                {"IF_LEFT", 0x2E },
                {"IF_NONE", 0x2F },
                {"INT", 0x30 },
                {"LAMBDA", 0x31 },
                {"LE", 0x32 },
                {"LEFT", 0x33 },
                {"LOOP", 0x34 },
                {"LSL", 0x35 },
                {"LSR", 0x36 },
                {"LT", 0x37 },
                {"MAP", 0x38 },
                {"MEM", 0x39 },
                {"MUL", 0x3A },
                {"NEG", 0x3B },
                {"NEQ", 0x3C },
                {"NIL", 0x3D },
                {"NONE", 0x3E },
                {"NOT", 0x3F },
                {"NOW", 0x40 },
                {"OR", 0x41 },
                {"PAIR", 0x42 },
                {"PUSH", 0x43 },
                {"RIGHT", 0x44 },
                {"SIZE", 0x45 },
                {"SOME", 0x46 },
                {"SOURCE", 0x47 },
                {"SENDER", 0x48 },
                {"SELF", 0x49 },
                {"STEPS_TO_QUOTA", 0x4A },
                {"SUB", 0x4B },
                {"SWAP", 0x4C },
                {"TRANSFER_TOKENS", 0x4D },
                {"SET_DELEGATE", 0x4E },
                {"UNIT", 0x4F },
                {"UPDATE", 0x50 },
                {"XOR", 0x51 },
                {"ITER", 0x52 },
                {"LOOP_LEFT", 0x53 },
                {"ADDRESS", 0x54 },
                {"CONTRACT", 0x55 },
                {"ISNAT", 0x56 },
                {"CAST", 0x57 },
                {"RENAME", 0x58 },
                {"bool", 0x59 },
                {"contract", 0x5A },
                {"int", 0x5B },
                {"key", 0x5C },
                {"key_hash", 0x5D },
                {"lambda", 0x5E },
                {"list", 0x5F },
                {"map", 0x60 },
                {"big_map", 0x61 },
                {"nat", 0x62 },
                {"option", 0x63 },
                {"or", 0x64 },
                {"pair", 0x65 },
                {"set", 0x66 },
                {"signature", 0x67 },
                {"string", 0x68 },
                {"bytes", 0x69 },
                {"mutez", 0x6A },
                {"timestamp", 0x6B },
                {"unit", 0x6C },
                {"operation", 0x6D },
                {"address", 0x6E },
                {"SLICE", 0x6F },
                {"DIG", 0x70 },
                {"DUG", 0x71 },
                {"EMPTY_BIG_MAP", 0x72 },
                {"APPLY", 0x73 },
                {"chain_id", 0x74 },
                {"CHAIN_ID", 0x75 },
            };
            #endregion
            
            switch (data)
            {
                case JArray _:
                    res.Add(0x02);
                    res.AddRange(ForgeArray(data.Select(ForgeMicheline).SelectMany(x => x).ToArray()).ToList());
//                    Console.WriteLine($"JArray {Hex.Convert(res.ToArray())}");
                    break;
                case JObject _ when data["prim"] != null:
                {
                    var argsLen = data["args"]?.Count() ?? 0;
                    var annotsLen = data["annots"]?.Count() ?? 0;

                    res.Add(lenTags[argsLen][annotsLen > 0]);
                    res.Add(primTags[data["prim"].ToString()]);
//                    Console.WriteLine($"Args {Hex.Convert(res.ToArray())}");

                    if (argsLen > 0)
                    {
                        var args = data["args"].Select(ForgeMicheline).SelectMany(x => x);
                        if (argsLen < 3)
                        {
                            res.AddRange(args.ToList());
//                            Console.WriteLine($"argsLen > 0 {Hex.Convert(res.ToArray())}");
                        }
                        else
                        {
                            res.AddRange(ForgeArray(args.ToArray()));
//                            Console.WriteLine($"argsLen <= 0 {Hex.Convert(res.ToArray())}");
                        }
                    }

                    if (annotsLen > 0)
                    {
                        res.AddRange(ForgeArray(Encoding.UTF8.GetBytes(string.Join(" ", data["annots"]))));
//                        Console.WriteLine($"annotsLen > 0 {Hex.Convert(res.ToArray())}");
                    }

                    else if (argsLen == 3)
                        res.AddRange(new List<byte>{0,0,0,0}); /* new string('0', 8);*/
//                    Console.WriteLine($"argsLen == 3 {Hex.Convert(res.ToArray())}");

                    break;
                }
                case JObject _ when data["bytes"] != null:
                    res.Add(0x0A);
                    res.AddRange(ForgeArray(Hex.Parse(data["bytes"].Value<string>())));
//                    Console.WriteLine($"Bytes {Hex.Convert(res.ToArray())}");
                    break;
                case JObject _ when data["int"] != null:
                    res.Add(0x00);
                    res.AddRange(ForgeInt(data["int"].Value<int>()));
//                    Console.WriteLine($"int {Hex.Convert(res.ToArray())}");
                    break;
                case JObject _ when data["string"] != null:
                    res.Add(0x01);
                    res.AddRange(ForgeArray(Encoding.UTF8.GetBytes(data["string"].Value<string>())));
//                    Console.WriteLine($"String {data["string"].Value<string>()} {Hex.Convert(res.ToArray())}");
                    break;
                case JObject _:
                    throw new ArgumentException($"Michelson forge error");
                default:
                    throw new ArgumentException($"Michelson forge error");
            }

            return res;
        }
    }
}