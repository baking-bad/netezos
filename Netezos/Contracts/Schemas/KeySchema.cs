using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class KeySchema : Schema, IFlat
    {
        public override PrimType Prim => PrimType.key;

        public KeySchema(MichelinePrim micheline) : base(micheline) { }

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
                var prefix = (micheBytes.Value[0], micheBytes.Value.Length) switch
                {
                    (0, 33) => Prefix.edpk,
                    (1, 34) => Prefix.sppk,
                    (2, 34) => Prefix.p2pk,
                    (3, 49) => Prefix.BLpk,
                    _ => null
                };

                if (prefix == null)
                    return Hex.Convert(micheBytes.Value);

                var bytes = micheBytes.Value.GetBytes(1, micheBytes.Value.Length - 1);
                return Base58.Convert(bytes, prefix);
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
                var bytes = Base58.Parse(micheStr.Value, 4);
                byte[] res;

                switch (micheStr.Value.Substring(0, 4))
                {
                    case "edpk":
                        res = new byte[33];
                        res[0] = 0;
                        break;
                    case "sppk":
                        res = new byte[34];
                        res[0] = 1;
                        break;
                    case "p2pk":
                        res = new byte[34];
                        res[0] = 2;
                        break;
                    case "BLpk":
                        res = new byte[49];
                        res[0] = 3;
                        break;
                    default:
                        throw FormatException(value);
                }

                bytes.CopyTo(res, 1);
                return new MichelineBytes(res);
            }

            return value;
        }
    }
}
