using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class BoolSchema : Schema, IFlat
    {
        public override PrimType Prim => PrimType.@bool;

        public BoolSchema(MichelinePrim micheline) : base(micheline) { }

        internal override void WriteValue(Utf8JsonWriter writer, IMicheline value)
        {
            writer.WriteStringValue(Flatten(value));
        }

        internal override void WriteJsonSchema(Utf8JsonWriter writer)
        {
            writer.WriteString("type", "string");

            writer.WriteStartArray("enum");
            writer.WriteStringValue("True");
            writer.WriteStringValue("False");
            writer.WriteEndArray();

            writer.WriteString("$comment", Prim.ToString());
        }

        public string Flatten(IMicheline value)
        {
            if (value is MichelinePrim michePrim
                && (michePrim.Prim == PrimType.True || michePrim.Prim == PrimType.False))
                return (michePrim.Prim == PrimType.True).ToString();

            throw FormatException(value);
        }

        protected override IMicheline MapValue(object value)
        {
            switch (value)
            {
                case bool b:
                    return new MichelinePrim { Prim = b ? PrimType.True : PrimType.False };
                case JsonElement json when json.ValueKind == JsonValueKind.True:
                    return new MichelinePrim { Prim = PrimType.True };
                case JsonElement json when json.ValueKind == JsonValueKind.False:
                    return new MichelinePrim { Prim = PrimType.False };
                case JsonElement json when json.ValueKind == JsonValueKind.String:
                    // TODO: validation
                    return new MichelinePrim { Prim = json.ValueEquals("True") ? PrimType.True : PrimType.False };
                default:
                    throw MapFailedException("invalid value");
            }
        }
    }
}
