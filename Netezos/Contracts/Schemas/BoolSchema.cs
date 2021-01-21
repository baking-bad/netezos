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

        public string Flatten(IMicheline value)
        {
            if (value is MichelinePrim michePrim
                && (michePrim.Prim == PrimType.True || michePrim.Prim == PrimType.False))
                return (michePrim.Prim == PrimType.True).ToString();

            throw FormatException(value);
        }
    }
}
