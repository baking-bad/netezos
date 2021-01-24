using System;
using System.Numerics;
using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class SaplingTransactionSchema : Schema, IFlat
    {
        public override PrimType Prim => PrimType.sapling_transaction;

        public override string Signature => $"sapling_transaction:{MemoSize}";

        public BigInteger MemoSize { get; }

        public SaplingTransactionSchema(MichelinePrim micheline) : base(micheline)
        {
            if (micheline.Args?.Count != 1 || !(micheline.Args[0] is MichelineInt micheInt))
                throw new FormatException($"Invalid {Prim} schema format");

            MemoSize = micheInt.Value;
        }

        internal override void WriteValue(Utf8JsonWriter writer, IMicheline value)
        {
            writer.WriteStringValue(Flatten(value));
        }

        public string Flatten(IMicheline value)
        {
            if (value is MichelineBytes micheBytes)
                return Hex.Convert(micheBytes.Value);

            throw FormatException(value);
        }
    }
}
