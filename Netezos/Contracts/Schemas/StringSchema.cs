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
            switch (value)
            {
                case string str:
                    return new MichelineString(str);
                case JsonElement json when json.ValueKind == JsonValueKind.String:
                    return new MichelineString(json.GetString());
                default:
                    throw MapFailedException("invalid value");
            }
        }
    }
}
