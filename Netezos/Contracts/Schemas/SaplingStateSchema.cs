using System;
using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class SaplingStateSchema : Schema
    {
        public override PrimType Prim => PrimType.sapling_state;

        public SaplingStateSchema(MichelinePrim micheline) : base(micheline) { }

        internal override void WriteProperty(Utf8JsonWriter writer, IMicheline value)
            => throw new NotImplementedException("Sapling state is not implemented yet");

        internal override void WriteValue(Utf8JsonWriter writer, IMicheline value)
            => throw new NotImplementedException("Sapling state is not implemented yet");
    }
}
