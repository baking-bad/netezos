using System;
using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class NeverSchema : Schema
    {
        public override PrimType Prim => PrimType.never;

        public NeverSchema(MichelinePrim micheline) : base(micheline) { }
        
        internal override void WriteProperty(Utf8JsonWriter writer, IMicheline value)
            => throw new InvalidOperationException($"Value of type {Prim} is not allowed");

        internal override void WriteValue(Utf8JsonWriter writer, IMicheline value)
            => throw new InvalidOperationException($"Value of type {Prim} is not allowed");
    }
}
