using System.Numerics;
using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class Bls12381FrSchema : Schema, IFlat
    {
        public override PrimType Prim => PrimType.bls12_381_fr;

        BigInteger Order => new(new byte[]
        {
            1, 0, 0, 0, 255, 255, 255, 255, 254, 91, 254, 255, 2, 164, 189, 83,
            5, 216, 161, 9, 8, 216, 57, 51, 72, 125, 157, 41, 83, 167, 237, 115
        });

        public Bls12381FrSchema(MichelinePrim micheline) : base(micheline) { }

        internal override void WriteValue(Utf8JsonWriter writer, IMicheline value)
        {
            writer.WriteStringValue(Flatten(value));
        }
        
        public string Flatten(IMicheline value)
        {
            if (value is MichelineInt micheInt)
            {
                var el = (micheInt.Value % Order + Order) % Order;
                return Hex.Convert(el.ToByteArray());
            }

            if (value is MichelineBytes micheBytes)
                return Hex.Convert(micheBytes.Value);

            throw FormatException(value);
        }

        protected override IMicheline MapValue(object value)
        {
            switch (value)
            {
                case BigInteger b:
                    return new MichelineBytes(b.ToByteArray());
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

        public override IMicheline Optimize(IMicheline value)
        {
            if (value is MichelineInt micheInt)
            {
                var el = (micheInt.Value % Order + Order) % Order;
                return new MichelineBytes(el.ToByteArray());
            }

            return value;
        }
    }
}
