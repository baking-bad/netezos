using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class ChainIdSchema : Schema, IFlat
    {
        public override PrimType Prim => PrimType.chain_id;

        public ChainIdSchema(MichelinePrim micheline) : base(micheline) { }

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
                if (micheBytes.Value.Length != 4)
                    return Hex.Convert(micheBytes.Value);

                return Base58.Convert(micheBytes.Value, Prefix.Net);
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
                var bytes = Base58.Parse(micheStr.Value, 3);
                return new MichelineBytes(bytes);
            }

            return value;
        }
    }
}
