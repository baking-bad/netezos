using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class AddressSchema : Schema, IFlat
    {
        public override PrimType Prim => PrimType.address;

        public AddressSchema(MichelinePrim micheline) : base(micheline) { }

        internal override void WriteValue(Utf8JsonWriter writer, IMicheline value)
        {
            writer.WriteStringValue(Flatten(value));
        }

        public string Flatten(IMicheline value)
        {
            if (value is MichelineString micheString)
            {
                return micheString.Value;
            }
            else if (value is MichelineBytes micheBytes)
            {
                if (micheBytes.Value.Length < 22)
                    return Hex.Convert(micheBytes.Value);

                byte[] prefix;
                if (micheBytes.Value[0] == 0)
                {
                    if (micheBytes.Value[1] == 0)
                        prefix = Prefix.tz1;
                    else if (micheBytes.Value[1] == 1)
                        prefix = Prefix.tz2;
                    else if (micheBytes.Value[1] == 2)
                        prefix = Prefix.tz3;
                    else if (micheBytes.Value[1] == 3)
                        prefix = Prefix.tz4;
                    else
                        return Hex.Convert(micheBytes.Value);
                }
                else if (micheBytes.Value[0] == 1)
                {
                    if (micheBytes.Value[21] == 0)
                        prefix = Prefix.KT1;
                    else
                        return Hex.Convert(micheBytes.Value);
                }
                else if (micheBytes.Value[0] == 2)
                {
                    if (micheBytes.Value[21] == 0)
                        prefix = Prefix.txr1;
                    else
                        return Hex.Convert(micheBytes.Value);
                }
                else if (micheBytes.Value[0] == 3)
                {
                    if (micheBytes.Value[21] == 0)
                        prefix = Prefix.sr1;
                    else
                        return Hex.Convert(micheBytes.Value);
                }
                else
                {
                    return Hex.Convert(micheBytes.Value);
                }

                var bytes = micheBytes.Value[0] == 0
                    ? micheBytes.Value.GetBytes(2, 20)
                    : micheBytes.Value.GetBytes(1, 20);

                var address = Base58.Convert(bytes, prefix);
                var entrypoint = micheBytes.Value.Length > 22
                    ? Utf8.Convert(micheBytes.Value.GetBytes(22, micheBytes.Value.Length - 22))
                    : string.Empty;

                return entrypoint.Length == 0 ? address : $"{address}%{entrypoint}";
            }
            else
            {
                throw FormatException(value);
            }
        }

        protected override IMicheline MapValue(object? value)
        {
            return value switch
            {
                string str => new MichelineString(str),
                byte[] bytes => new MichelineBytes(bytes),
                JsonElement { ValueKind: JsonValueKind.String } json => new MichelineString(json.GetString()!),
                _ => throw MapFailedException("invalid value"),
            };
        }

        public override IMicheline Optimize(IMicheline value)
        {
            if (value is not MichelineString micheStr)
                return value;

            string address;
            byte[] addressBytes;
            byte[]? entrypointBytes;

            if (micheStr.Value.StartsWith("txr1"))
            {
                address = micheStr.Value.Length > 37 ? micheStr.Value.Substring(0, 37) : micheStr.Value;
                addressBytes = Base58.Parse(address, 4);
                entrypointBytes = micheStr.Value.Length > 38
                    ? Utf8.Parse(micheStr.Value.Substring(38))
                    : null;
            }
            else
            {
                address = micheStr.Value.Length > 36 ? micheStr.Value.Substring(0, 36) : micheStr.Value;
                addressBytes = Base58.Parse(address, 3);
                entrypointBytes = micheStr.Value.Length > 37
                    ? Utf8.Parse(micheStr.Value.Substring(37))
                    : null;
            }

            var res = new byte[22 + (entrypointBytes?.Length ?? 0)];

            switch (address.Substring(0, 3))
            {
                case "tz1":
                    addressBytes.CopyTo(res, 2);
                    res[0] = 0;
                    res[1] = 0;
                    break;
                case "tz2":
                    addressBytes.CopyTo(res, 2);
                    res[0] = 0;
                    res[1] = 1;
                    break;
                case "tz3":
                    addressBytes.CopyTo(res, 2);
                    res[0] = 0;
                    res[1] = 2;
                    break;
                case "tz4":
                    addressBytes.CopyTo(res, 2);
                    res[0] = 0;
                    res[1] = 3;
                    break;
                case "KT1":
                    addressBytes.CopyTo(res, 1);
                    res[0] = 1;
                    res[21] = 0;
                    break;
                case "txr" when address.StartsWith("txr1"):
                    addressBytes.CopyTo(res, 1);
                    res[0] = 2;
                    res[21] = 0;
                    break;
                case "sr1":
                    addressBytes.CopyTo(res, 1);
                    res[0] = 3;
                    res[21] = 0;
                    break;
                default:
                    throw FormatException(value);
            }

            entrypointBytes?.CopyTo(res, 22);

            return new MichelineBytes(res);
        }
    }
}
