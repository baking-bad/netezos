using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class SaplingStateSchema : Schema
    {
        public override PrimType Prim => PrimType.sapling_state;

        public override string Signature => $"sapling_state:{MemoSize}";

        public BigInteger MemoSize { get; }

        public SaplingStateSchema(MichelinePrim micheline) : base(micheline)
        {
            if (micheline.Args?.Count != 1 || !(micheline.Args[0] is MichelineInt micheInt))
                throw new FormatException($"Invalid {Prim} schema format");

            MemoSize = micheInt.Value;
        }

        internal override void WriteProperty(Utf8JsonWriter writer, IMicheline value)
            => throw new NotImplementedException("Sapling state is not implemented yet");

        internal override void WriteValue(Utf8JsonWriter writer, IMicheline value)
            => throw new NotImplementedException("Sapling state is not implemented yet");

        protected override List<IMicheline> GetArgs()
        {
            return new List<IMicheline>(1) { new MichelineInt(MemoSize) };
        }
    }
}
