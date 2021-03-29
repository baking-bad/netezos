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
            if (!(value is MichelinePrim michePrim)
                || (michePrim.Prim != PrimType.True && michePrim.Prim != PrimType.False))
                throw FormatException(value);
            
            writer.WriteBooleanValue(michePrim.Prim == PrimType.True);
        }

        internal override void WriteJsonSchema(Utf8JsonWriter writer)
        {
            writer.WriteString("type", "boolean");
            writer.WriteString("$comment", Prim.ToString());
        }

        public string Flatten(IMicheline value)
        {
            if (value is MichelinePrim michePrim
                && (michePrim.Prim == PrimType.True || michePrim.Prim == PrimType.False))
                return (michePrim.Prim == PrimType.True).ToString().ToLower();

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
                default:
                    throw MapFailedException("invalid value");
            }
        }
    }
}
