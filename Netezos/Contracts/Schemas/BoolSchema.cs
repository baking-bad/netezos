using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class BoolSchema : Schema
    {
        public override PrimType Prim => PrimType.@bool;

        public BoolSchema(MichelinePrim micheline) : base(micheline) { }

        internal override void WriteValue(Utf8JsonWriter writer, IMicheline value)
        {
            if (value is MichelinePrim michePrim
                && (michePrim.Prim == PrimType.True || michePrim.Prim == PrimType.False))
            {
                writer.WriteBooleanValue(michePrim.Prim == PrimType.True);
            }
            else
            {
                throw FormatException(value);
            }
        }
    }
}
