using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class KeyHashSchema : Schema, IFlat
    {
        public override PrimType Prim => PrimType.key_hash;

        public KeyHashSchema(MichelinePrim micheline) : base(micheline) { }

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
                if (micheBytes.Value.Length != 21)
                    return Hex.Convert(micheBytes.Value);

                var prefix = micheBytes.Value[0] switch
                {
                    0 => Prefix.tz1,
                    1 => Prefix.tz2,
                    2 => Prefix.tz3,
                    3 => Prefix.tz4,
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
                var bytes = Base58.Parse(micheStr.Value, 3);
                var res = new byte[21];

                res[0] = micheStr.Value.Substring(0, 3) switch
                {
                    "tz1" => 0,
                    "tz2" => 1,
                    "tz3" => 2,
                    "tz4" => 3,
                    _ => throw FormatException(value)
                };

                bytes.CopyTo(res, 1);
                return new MichelineBytes(res);
            }

            return value;
        }
    }
}
