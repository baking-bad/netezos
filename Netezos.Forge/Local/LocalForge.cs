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
        private static readonly byte[] BranchPrefix = { 1, 52 };
        public Task<byte[]> ForgeOperationAsync(string branch, OperationContent content)
        {
            var res = string.IsNullOrWhiteSpace(branch) ? "" : Hex.Convert(Base58.Parse(branch, BranchPrefix));

            switch (content.Kind)
            {
                case "transaction":
                    res += ForgeTransaction((TransactionContent) content);
                    break;
                case "reveal":
                    res += ForgeRevelation((RevealContent) content);
                    break;
                case "activate_account":
                    res += ForgeActivation((ActivationContent) content);
                    break;
                default:
                    throw new NotImplementedException($"{content.Kind} is not implemented");
            }
            
            return Task.FromResult(Hex.Parse(res));
        }

        public Task<byte[]> ForgeOperationGroupAsync(string branch, IEnumerable<ManagerOperationContent> contents)
        {
            var res = string.IsNullOrWhiteSpace(branch) ? "" : Hex.Convert(Base58.Parse(branch, BranchPrefix));

            foreach (var operation in contents)
            {
                switch (operation.Kind)
                {
                    case "transaction":
                        res += ForgeTransaction((TransactionContent)operation);
                        break;
                    case "reveal":
                        res += ForgeRevelation((RevealContent) operation);
                        break;
                        
                    default:
                        throw new NotImplementedException($"{operation.Kind} is not implemented");
                }
            }
            
            
            return Task.FromResult(Hex.Parse(res));
        }

        private string ForgeRevelation(RevealContent operation)
        {
            var res = ForgeLong(107);
            res += ForgeSource(operation.Source);
            res += ForgeLong(operation.Fee);
            res += ForgeLong(operation.Counter);
            res += ForgeLong(operation.GasLimit);
            res += ForgeLong(operation.StorageLimit);
            res += ForgePublicKey(operation.PublicKey);

            return res;
        }

        private string ForgeActivation(ActivationContent operation)
        {
            var res = ForgeLong(4);
            res += ForgeActivationAddress(operation.Address);
            res += operation.Secret;

            return res;
        }

        private string ForgeTransaction(TransactionContent operation)
        {
            var res = ForgeLong(108);
            res += ForgeSource(operation.Source);
            res += ForgeLong(operation.Fee);
            res += ForgeLong(operation.Counter);
            res += ForgeLong(operation.GasLimit);
            res += ForgeLong(operation.StorageLimit);
            res += ForgeLong(operation.Amount);
            res += ForgeAddress(operation.Destination);

            if (operation.Parameters != null)
            {
                res += ForgeBool(true);
                res += ForgeEntrypoint(operation.Parameters.Entrypoint);
                res += ForgeArray(ForgeMicheline(operation.Parameters.Value));
            }
            else
                res += ForgeBool(false);

            return res;
        }
        
        
        
        
        private static string ForgeArray(string value)
        {
            var bytes = BitConverter.GetBytes(value.Length / 2).Reverse().ToArray();
            return Hex.Convert(bytes) + value;
        }
        private string ForgeLong(long value)
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

            return Hex.Convert(buf.ToArray());
        }
        
        private static string ForgeAddress(string value)
        {
            var prefix = value.Substring(0, 3);

            var res = Hex.Convert(Base58.Parse(value)).Substring(6);

            switch (prefix)
            {
                case "tz1":
                    res = "0000" + res;
                    break;
                case "tz2":
                    res = "0001" + res;
                    break;
                case "tz3":
                    res = "0002" + res;
                    break;
                case "KT1":
                    res = "01" + res + "00";
                    break;
                default:
                    throw new ArgumentException($"Value address exception. Invalid prefix {prefix}");
            }

            return res;
        }
        
        private static string ForgeActivationAddress(string value)
        {
            return Hex.Convert(Base58.Parse(value)).Substring(6);;
        }

        private static string ForgeSource(string value)
        {
            var prefix = value.Substring(0, 3);
            
            var res = Hex.Convert(Base58.Parse(value)).Substring(6);

            switch (prefix)
            {
                case "tz1":
                    res = "00" + res;
                    break;
                case "tz2":
                    res = "01" + res;
                    break;
                case "tz3":
                    res = "02" + res;
                    break;
                default:
                    throw new ArgumentException($"Value address exception. Invalid prefix {prefix}");
            }

            return res;
        }

        private static string ForgeBool(bool value)
        {
            return value ? "FF" : "00";
        }

        private static string ForgePublicKey(string value)
        {
            var prefix = value.Substring(0, 4);
            var res = Hex.Convert(Base58.Parse(value)).Substring(8);

            switch (prefix)
            {
                case "edpk":
                    res = "00" + res;
                    break;
                case "sppk":
                    res = "01" + res;
                    break;
                case "p2pk":
                    res = "02" + res;
                    break;
                default:
                    throw new ArgumentException($"Value address exception. Invalid prefix {prefix}");
            }

            return res;
        }
        
        private static string ForgeInt(int value)
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

            var res = "";

            for (int i = 0; i < septets.Count; i++)
            {
                string prefix = i == septets.Count - 1 ? "0" : "1";
                res += Convert.ToByte(prefix + septets[i], 2).ToString("X2");
            }

            return res;
        }

        private static string ForgeEntrypoint(string value)
        {
            var res = "";

            var entrypointTags = new Dictionary<string, int>
            {
                {"default", 0},
                {"root", 1},
                {"do", 2},
                {"set_delegate", 3},
                {"remove_delegate", 4}
            };
            if (entrypointTags.ContainsKey(value))
            {
                res += entrypointTags[value].ToString("X2");
            }
            else
            {
                res += "ff";
                res += ForgeArray(Hex.Convert(Encoding.Default.GetBytes(value)));
            }

            return res;
        }

        private static string ForgeMicheline(JToken data)
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