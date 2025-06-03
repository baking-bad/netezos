using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class BoolSchema(MichelinePrim micheline) : Schema(micheline), IFlat
    {
        public override PrimType Prim => PrimType.@bool;

        internal override void WriteValue(Utf8JsonWriter writer, IMicheline value)
        {
            if (value is not MichelinePrim michePrim
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

        protected override IMicheline MapValue(object? value)
        {
            return value switch
            {
                bool b => new MichelinePrim { Prim = b ? PrimType.True : PrimType.False },
                JsonElement { ValueKind: JsonValueKind.True } => new MichelinePrim { Prim = PrimType.True },
                JsonElement { ValueKind: JsonValueKind.False } => new MichelinePrim { Prim = PrimType.False },
                _ => throw MapFailedException("invalid value")
            };
        }
    }
}
