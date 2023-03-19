using System.Text.Json;
using Netezos.Encoding;
using Netezos.Forging;
using Netezos.Utils;

namespace Netezos.Contracts
{
    public sealed class ConstantSchema : Schema
    {
        public override PrimType Prim => PrimType.constant;

        public ConstantSchema(MichelinePrim micheline) : base(micheline) { }

        internal override void WriteProperty(Utf8JsonWriter writer, IMicheline value)
            => throw new InvalidOperationException($"Value of type {Prim} is not allowed");

        internal override void WriteValue(Utf8JsonWriter writer, IMicheline value)
            => throw new InvalidOperationException($"Value of type {Prim} is not allowed");

        public override IMicheline MapObject(object obj, bool isValue = false)
            => throw new InvalidOperationException($"Value of type {Prim} is not allowed");

        protected override IMicheline MapValue(object value)
            => throw new InvalidOperationException($"Value of type {Prim} is not allowed");

        public static string GetGlobalAddress(IMicheline value)
        {
            var bytes = LocalForge.ForgeMicheline(value);
            var hash = Blake2b.GetDigest(bytes);
            return Base58.Convert(hash, Prefix.expr);
        }
    }
}
