using System.Numerics;
using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class MutezSchema(MichelinePrim micheline) : Schema(micheline), IFlat
    {
        public override PrimType Prim => PrimType.mutez;

        internal override void WriteValue(Utf8JsonWriter writer, IMicheline value)
        {
            writer.WriteStringValue(Flatten(value));
        }

        public string Flatten(IMicheline value)
        {
            if (value is MichelineInt micheInt)
                return micheInt.Value.ToString();

            throw FormatException(value);
        }

        protected override IMicheline MapValue(object? value)
        {
            return value switch
            {
                BigInteger b => new MichelineInt(b),
                int i => new MichelineInt(i),
                long l => new MichelineInt(l),
                string s => new MichelineInt(BigInteger.Parse(s)),
                JsonElement { ValueKind: JsonValueKind.Number } json => new MichelineInt(new BigInteger(json.GetInt64())),
                JsonElement { ValueKind: JsonValueKind.String } json => new MichelineInt(BigInteger.Parse(json.GetString()!)),
                _ => throw MapFailedException("invalid value")
            };
        }
    }
}
