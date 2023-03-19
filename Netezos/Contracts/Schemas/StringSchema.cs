using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class StringSchema : Schema, IFlat
    {
        public override PrimType Prim => PrimType.@string;

        public StringSchema(MichelinePrim micheline) : base(micheline) { }

        internal override void WriteValue(Utf8JsonWriter writer, IMicheline value)
        {
            writer.WriteStringValue(Flatten(value));
        }

        public string Flatten(IMicheline value)
        {
            if (value is MichelineString micheString)
                return micheString.Value;

            throw FormatException(value);
        }

        protected override IMicheline MapValue(object value)
        {
            return value switch
            {
                string str => new MichelineString(str),
                JsonElement { ValueKind: JsonValueKind.String } json => new MichelineString(json.GetString()!),
                _ => throw MapFailedException("invalid value")
            };
        }
    }
}
