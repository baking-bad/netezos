using System;
using System.Collections.Generic;
using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class StorageSchema : Schema
    {
        public override PrimType Prim => PrimType.storage;

        public override string Signature => Schema.Signature;

        public Schema Schema { get; }

        public StorageSchema(MichelinePrim micheline) : base(micheline)
        {
            if (micheline.Args?.Count != 1
                || !(micheline.Args[0] is MichelinePrim storage))
                throw new FormatException($"Invalid {Prim} schema format");

            Schema = Create(storage);
        }

        internal override void WriteProperty(Utf8JsonWriter writer)
        {
            Schema.WriteProperty(writer);
        }

        internal override void WriteProperty(Utf8JsonWriter writer, IMicheline value)
        {
            Schema.WriteProperty(writer, value);
        }
        
        internal override void WriteValue(Utf8JsonWriter writer)
        {
            Schema.WriteValue(writer);
        }

        internal override void WriteValue(Utf8JsonWriter writer, IMicheline value)
        {
            Schema.WriteValue(writer, value);
        }

        protected override List<IMicheline> GetArgs()
        {
            return new List<IMicheline>(1) { Schema.ToMicheline() };
        }
    }
}
