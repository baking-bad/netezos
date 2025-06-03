using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class ChestKeySchema : Schema, IFlat
    {
        public override PrimType Prim => PrimType.chest_key;

        public ChestKeySchema(MichelinePrim micheline) : base(micheline) { }

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

        protected override IMicheline MapValue(object? value)
        {
            switch (value)
            {
                case byte[] bytes:
                    return new MichelineBytes(bytes);
                case string str:
                    if (!Hex.TryParse(str, out var b1))
                        throw MapFailedException($"invalid hex string");
                    return new MichelineBytes(b1);
                case JsonElement json when json.ValueKind == JsonValueKind.String:
                    if (!Hex.TryParse(json.GetString()!, out var b2))
                        throw MapFailedException($"invalid hex string");
                    return new MichelineBytes(b2);
                default:
                    throw MapFailedException("invalid value");
            }
        }
    }
}
