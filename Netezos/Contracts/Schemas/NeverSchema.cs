using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class NeverSchema(MichelinePrim micheline) : Schema(micheline)
    {
        public override PrimType Prim => PrimType.never;

        internal override void WriteProperty(Utf8JsonWriter writer, IMicheline value)
            => throw new InvalidOperationException($"Value of type {Prim} is not allowed");

        internal override void WriteValue(Utf8JsonWriter writer, IMicheline value)
            => throw new InvalidOperationException($"Value of type {Prim} is not allowed");

        public override IMicheline MapObject(object? obj, bool isValue = false)
            => throw new InvalidOperationException($"Value of type {Prim} is not allowed");

        protected override IMicheline MapValue(object? value)
            => throw new InvalidOperationException($"Value of type {Prim} is not allowed");
    }
}
