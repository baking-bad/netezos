using System;
using System.Collections.Generic;
using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class OptionSchema : Schema
    {
        public override PrimType Prim => PrimType.option;

        public override string Name => (Field ?? Type
            ?? Some.Field ?? Some.Type
            ?? Some.Prim.ToString())
            + Suffix;

        public override string Signature => $"?{Some.Signature}";

        public Schema Some { get; }

        public OptionSchema(MichelinePrim micheline) : base(micheline)
        {
            if (micheline.Args?.Count != 1 || !(micheline.Args[0] is MichelinePrim some))
                throw new FormatException($"Invalid {Prim} schema format");

            Some = Create(some);
        }

        internal override void WriteValue(Utf8JsonWriter writer)
        {
            Some.WriteValue(writer);
        }

        internal override void WriteValue(Utf8JsonWriter writer, IMicheline value)
        {
            if (!(value is MichelinePrim prim))
                throw FormatException(value);

            if (prim.Prim == PrimType.None)
            {
                writer.WriteNullValue();
            }
            else if (prim.Prim == PrimType.Some)
            {
                if (prim.Args?.Count != 1)
                    throw new FormatException("Invalid 'Some' prim args count");

                Some.WriteValue(writer, prim.Args[0]);
            }
            else
            {
                throw FormatException(value);
            }
        }

        protected override List<IMicheline> GetArgs()
        {
            return new List<IMicheline>(1) { Some.ToMicheline() };
        }
    }
}
