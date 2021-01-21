using System.Numerics;
using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class Bls12381FrSchema : Schema, IFlat
    {
        public override PrimType Prim => PrimType.bls12_381_fr;

        public Bls12381FrSchema(MichelinePrim micheline) : base(micheline) { }

        internal override void WriteValue(Utf8JsonWriter writer, IMicheline value)
        {
            writer.WriteStringValue(Flatten(value));
        }
        
        public string Flatten(IMicheline value)
        {
            if (value is MichelineInt micheInt)
                return micheInt.Value.ToString();

            if (value is MichelineBytes micheBytes)
                return new BigInteger(micheBytes.Value).ToString();

            throw FormatException(value);
        }
    }
}
