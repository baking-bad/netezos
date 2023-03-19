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
            if (value is MichelineString micheStr)
            {
                var res = micheStr.Value.Substring(0, 3) switch
                {
                    "eds" or "sps" => Base58.Parse(micheStr.Value, 5),
                    "p2s" or "BLs" => Base58.Parse(micheStr.Value, 4),
                    "sig" => Base58.Parse(micheStr.Value, 3),
                    _ => throw FormatException(value)
                };
                return new MichelineBytes(res);
            }

            return value;
        }
    }
}
