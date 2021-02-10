using System.Numerics;
using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class Bls12381G1Schema : Schema, IFlat
    {
        public override PrimType Prim => PrimType.bls12_381_g1;

        public Bls12381G1Schema(MichelinePrim micheline) : base(micheline) { }

        internal override void WriteValue(Utf8JsonWriter writer, IMicheline value)
        {
            writer.WriteStringValue(Flatten(value));
        }

        public string Flatten(IMicheline value)
        {
            if (value is MichelineInt micheInt)
                return Hex.Convert(micheInt.Value.ToByteArray());

            if (value is MichelineBytes micheBytes)
                return Hex.Convert(micheBytes.Value);

            throw FormatException(value);
        }

        protected override IMicheline MapValue(object value)
        {
            switch (value)
            {
                case BigInteger b:
                    return new MichelineInt(b);
                case int i:
                    return new MichelineInt(new BigInteger(i));
                case long l:
                    return new MichelineInt(new BigInteger(l));
                case string s:
                    // TODO: validation
                    return new MichelineInt(BigInteger.Parse(s));
                case JsonElement json when json.ValueKind == JsonValueKind.Number:
                    // TODO: validation
                    return new MichelineInt(new BigInteger(json.GetInt64()));
                case JsonElement json when json.ValueKind == JsonValueKind.String:
                    // TODO: validation
                    return new MichelineInt(BigInteger.Parse(json.GetString()));
                default:
                    throw MapFailedException("invalid value");
            }
        }
    }
}
