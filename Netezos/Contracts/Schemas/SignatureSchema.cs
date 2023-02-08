using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class SignatureSchema : Schema, IFlat
    {
        public override PrimType Prim => PrimType.signature;

        public SignatureSchema(MichelinePrim micheline) : base(micheline) { }

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
                if (micheBytes.Value.Length != 64)
                    return Hex.Convert(micheBytes.Value);

                return Base58.Convert(micheBytes.Value, Prefix.sig);
            }
            else
            {
                throw FormatException(value);
            }
        }

        protected override IMicheline MapValue(object value)
        {
            switch (value)
            {
                case string str:
                    // TODO: validation & optimization
                    return new MichelineString(str);
                case byte[] bytes:
                    // TODO: validation
                    return new MichelineBytes(bytes);
                case JsonElement json when json.ValueKind == JsonValueKind.String:
                    // TODO: validation & optimization
                    return new MichelineString(json.GetString());
                default:
                    throw MapFailedException("invalid value");
            }
        }

        public override IMicheline Optimize(IMicheline value)
        {
            if (value is MichelineString micheStr)
            {
                byte[] res;
                switch (micheStr.Value.Substring(0, 3))
                {
                    case "eds":
                    case "sps":
                        res = Base58.Parse(micheStr.Value, 5);
                        break;
                    case "p2s":
                    case "BLs":
                        res = Base58.Parse(micheStr.Value, 4);
                        break;
                    case "sig":
                        res = Base58.Parse(micheStr.Value, 3);
                        break;
                    default:
                        throw FormatException(value);
                }
                return new MichelineBytes(res);
            }

            return value;
        }
    }
}
