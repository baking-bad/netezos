using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class UnitSchema(MichelinePrim micheline) : Schema(micheline)
    {
        public override PrimType Prim => PrimType.unit;

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

        internal override void WriteJsonSchema(Utf8JsonWriter writer)
        {
            writer.WriteString("type", "object");
            writer.WriteBoolean("additionalProperties", false);
            writer.WriteString("$comment", Prim.ToString());
        }

        public override IMicheline MapObject(object? value, bool isValue = false)
        {
            return new MichelinePrim { Prim = PrimType.Unit };
        }
    }
}
