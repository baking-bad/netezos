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

        internal override void WriteValue(Utf8JsonWriter writer, IMicheline value)
        {
            if (value is MichelineInt micheInt)
            {
                writer.WriteStringValue(micheInt.Value.ToString());
            }
            else if (value is MichelineArray micheArray)
            {
                if (micheArray.Count > 0)
                    throw new NotImplementedException("At the time of creation there was no any documentation on possible values of sapling_state :(");

                writer.WriteStartArray();
                writer.WriteEndArray();
            }
            else
            {
                throw FormatException(value);
            }
        }

        protected override List<IMicheline> GetArgs()
        {
            return new List<IMicheline>(1) { new MichelineInt(MemoSize) };
        }
    }
}
