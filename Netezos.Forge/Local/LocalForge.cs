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

        byte[] ForgeOperation(OperationContent content)
        {
            switch (content)
            {
                case TransactionContent transaction:
                    return ForgeTransaction(transaction);
                case RevealContent reveal:
                    return ForgeRevelation(reveal);
                case ActivationContent activation:
                    return ForgeActivation(activation);
                default:
                    throw new NotImplementedException($"{content.Kind} is not implemented");
            }
        }

        static byte[] ForgeRevelation(RevealContent operation)
        {
            var res = ForgeLong(107);
            res = res.Concat(ForgeSource(operation.Source));
            res = res.Concat(ForgeLong(operation.Fee));
            res = res.Concat(ForgeLong(operation.Counter));
            res = res.Concat(ForgeLong(operation.GasLimit));
            res = res.Concat(ForgeLong(operation.StorageLimit));
            res = res.Concat(ForgePublicKey(operation.PublicKey));

            return res;
        }

        static byte[] ForgeActivation(ActivationContent operation)
        {
            var res = ForgeLong(4);
            res = res.Concat(ForgeActivationAddress(operation.Address));
            res = res.Concat(Hex.Parse(operation.Secret));

            return res;
        }

        static byte[] ForgeTransaction(TransactionContent operation)
        {
            var res = ForgeLong(108);
            res = res.Concat(ForgeSource(operation.Source));
            res = res.Concat(ForgeLong(operation.Fee));
            res = res.Concat(ForgeLong(operation.Counter));
            res = res.Concat(ForgeLong(operation.GasLimit));
            res = res.Concat(ForgeLong(operation.StorageLimit));
            res = res.Concat(ForgeLong(operation.Amount));
            res = res.Concat(ForgeAddress(operation.Destination));

            if (operation.Parameters != null)
            {
                res = res.Concat(ForgeBool(true));
                res = res.Concat(ForgeEntrypoint(operation.Parameters.Entrypoint));
                res = res.Concat(Hex.Parse(ForgeArray(ForgeMicheline(operation.Parameters.Value))));
            }
            else
                res = res.Concat(ForgeBool(false));

            return res;
        }
        
        
        
        
        static string ForgeArray(string value)
        {
            var bytes = BitConverter.GetBytes(value.Length / 2).Reverse().ToArray();
            return Hex.Convert(bytes) + value;
        }
        static byte[] ForgeLong(long value)
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
                res = res.Concat(Hex.Parse(ForgeArray(value)));
            }

            return res;
        }

        static string ForgeMicheline(JToken data)
        {
            var res = "";

            #region Tags
            var lenTags = new Dictionary<bool, string>[] {
                new Dictionary<bool, string> {
                    { false, "03" },
                    { true, "04" }
                },
                new Dictionary<bool, string> {
                    { false, "05" },
                    { true, "06" }
                },
                new Dictionary<bool, string> {
                    { false, "07" },
                    { true, "08" }
                },
                new Dictionary<bool, string> {
                    { false, "09" },
                    { true, "09" }
                }
            };
            
            var primTags = new Dictionary<string, string> {
                {"parameter", "00" },
                {"storage", "01" },
                {"code", "02" },
                {"False", "03" },
                {"Elt", "04" },
                {"Left", "05" },
                {"None", "06" },
                {"Pair", "07" },
                {"Right", "08" },
                {"Some", "09" },
                {"True", "0A" },
                {"Unit", "0B" },
                {"PACK", "0C" },
                {"UNPACK", "0D" },
                {"BLAKE2B", "0E" },
                {"SHA256", "0F" },
                {"SHA512", "10" },
                {"ABS", "11" },
                {"ADD", "12" },
                {"AMOUNT", "13" },
                {"AND", "14" },
                {"BALANCE", "15" },
                {"CAR", "16" },
                {"CDR", "17" },
                {"CHECK_SIGNATURE", "18" },
                {"COMPARE", "19" },
                {"CONCAT", "1A" },
                {"CONS", "1B" },
                {"CREATE_ACCOUNT", "1C" },
                {"CREATE_CONTRACT", "1D" },
                {"IMPLICIT_ACCOUNT", "1E" },
                {"DIP", "1F" },
                {"DROP", "20" },
                {"DUP", "21" },
                {"EDIV", "22" },
                {"EMPTY_MAP", "23" },
                {"EMPTY_SET", "24" },
                {"EQ", "25" },
                {"EXEC", "26" },
                {"FAILWITH", "27" },
                {"GE", "28" },
                {"GET", "29" },
                {"GT", "2A" },
                {"HASH_KEY", "2B" },
                {"IF", "2C" },
                {"IF_CONS", "2D" },
                {"IF_LEFT", "2E" },
                {"IF_NONE", "2F" },
                {"INT", "30" },
                {"LAMBDA", "31" },
                {"LE", "32" },
                {"LEFT", "33" },
                {"LOOP", "34" },
                {"LSL", "35" },
                {"LSR", "36" },
                {"LT", "37" },
                {"MAP", "38" },
                {"MEM", "39" },
                {"MUL", "3A" },
                {"NEG", "3B" },
                {"NEQ", "3C" },
                {"NIL", "3D" },
                {"NONE", "3E" },
                {"NOT", "3F" },
                {"NOW", "40" },
                {"OR", "41" },
                {"PAIR", "42" },
                {"PUSH", "43" },
                {"RIGHT", "44" },
                {"SIZE", "45" },
                {"SOME", "46" },
                {"SOURCE", "47" },
                {"SENDER", "48" },
                {"SELF", "49" },
                {"STEPS_TO_QUOTA", "4A" },
                {"SUB", "4B" },
                {"SWAP", "4C" },
                {"TRANSFER_TOKENS", "4D" },
                {"SET_DELEGATE", "4E" },
                {"UNIT", "4F" },
                {"UPDATE", "50" },
                {"XOR", "51" },
                {"ITER", "52" },
                {"LOOP_LEFT", "53" },
                {"ADDRESS", "54" },
                {"CONTRACT", "55" },
                {"ISNAT", "56" },
                {"CAST", "57" },
                {"RENAME", "58" },
                {"bool", "59" },
                {"contract", "5A" },
                {"int", "5B" },
                {"key", "5C" },
                {"key_hash", "5D" },
                {"lambda", "5E" },
                {"list", "5F" },
                {"map", "60" },
                {"big_map", "61" },
                {"nat", "62" },
                {"option", "63" },
                {"or", "64" },
                {"pair", "65" },
                {"set", "66" },
                {"signature", "67" },
                {"string", "68" },
                {"bytes", "69" },
                {"mutez", "6A" },
                {"timestamp", "6B" },
                {"unit", "6C" },
                {"operation", "6D" },
                {"address", "6E" },
                {"SLICE", "6F" }
            };
            #endregion
            


            switch (data)
            {
                case JArray _:
                    res += "02";
                    res += ForgeArray(string.Concat(data.Select(ForgeMicheline)));
                    break;
                case JObject _ when data["prim"] != null:
                {
                    var argsLen = data["args"]?.Count() ?? 0;
                    var annotsLen = data["annots"]?.Count() ?? 0;

                    res += lenTags[argsLen][annotsLen > 0];
                    res += primTags[data["prim"].ToString()];

                    if (argsLen > 0)
                    {
                        var args = string.Concat(data["args"].Select(ForgeMicheline));
                        if (argsLen < 3)
                            res += args;
                        else
                            res += ForgeArray(args);
                    }

                    if (annotsLen > 0)
                        res += ForgeArray(string.Join(" ", data["annots"]));
                    else if (argsLen == 3)
                        res += new string('0', 8);
                    break;
                }
                case JObject _ when data["bytes"] != null:
                    res += "0A";
                    res += ForgeArray(data["bytes"].ToString());
                    break;
                case JObject _ when data["int"] != null:
                    res += "00";
                    res += ForgeInt(data["int"].Value<int>());
                    break;
                case JObject _ when data["string"] != null:
                    res += "01";
                    res += ForgeArray(Hex.Convert(Encoding.Default.GetBytes(data["string"].Value<string>())));
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