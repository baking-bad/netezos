using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class LambdaSchema : Schema
    {
        public override PrimType Prim => PrimType.lambda;

        public LambdaSchema(MichelinePrim micheline) : base(micheline) { }

        internal override void WriteValue(Utf8JsonWriter writer, IMicheline value)
        {
            // TODO: convert lambda to Michelson
            writer.WriteStringValue(Micheline.ToJson(value));
        }
    }
}
