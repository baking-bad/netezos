using System;
using System.Collections.Generic;
using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class LambdaSchema : Schema
    {
        public override PrimType Prim => PrimType.lambda;

        public Schema In { get; }
        public Schema Out { get; }

        public LambdaSchema(MichelinePrim micheline) : base(micheline)
        {
            if (micheline.Args?.Count != 2
                || !(micheline.Args[0] is MichelinePrim input)
                || !(micheline.Args[1] is MichelinePrim output))
                throw new FormatException($"Invalid {Prim} schema format");

            In = Create(input);
            Out = Create(output);
        }

        internal override void WriteValue(Utf8JsonWriter writer, IMicheline value)
        {
            // TODO: convert lambda to Michelson
            writer.WriteStringValue(Micheline.ToJson(value));
        }

        protected override List<IMicheline> GetArgs()
        {
            return new List<IMicheline>(2) { In.ToMicheline(), Out.ToMicheline() };
        }
    }
}
