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
    }
}
