using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class UnitSchema : Schema
    {
        public override PrimType Prim => PrimType.unit;

        public UnitSchema(MichelinePrim micheline) : base(micheline) { }

        internal override void WriteValue(Utf8JsonWriter writer, IMicheline value)
        {
            if (value is MichelinePrim prim && prim.Prim == PrimType.Unit)
            {
                writer.WriteStartObject();
                writer.WriteEndObject();
            }
            else
            {
                throw FormatException(value);
            }                
        }
    }
}
