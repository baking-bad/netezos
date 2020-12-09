using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class BytesSchema : Schema, IFlat
    {
        public override PrimType Prim => PrimType.bytes;

        public BytesSchema(MichelinePrim micheline) : base(micheline) { }

        internal override void WriteValue(Utf8JsonWriter writer, IMicheline value)
        {
            writer.WriteStringValue(Flatten(value));
        }

        public string Flatten(IMicheline value)
        {
            if (value is MichelineBytes micheBytes)
                return Hex.Convert(micheBytes.Value);

            throw FormatException(value);
        }
    }
}
