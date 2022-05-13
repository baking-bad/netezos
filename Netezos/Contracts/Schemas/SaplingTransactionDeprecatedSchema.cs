﻿using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class SaplingTransactionDeprecatedSchema : Schema, IFlat
    {
        public override PrimType Prim => PrimType.sapling_transaction_deprecated;

        public override string Signature => $"sapling_transaction_deprecated:{MemoSize}";

        public BigInteger MemoSize { get; }

        public SaplingTransactionDeprecatedSchema(MichelinePrim micheline) : base(micheline)
        {
            if (micheline.Args?.Count != 1 || micheline.Args[0] is not MichelineInt micheInt)
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

        protected override List<IMicheline> GetArgs()
        {
            return new List<IMicheline>(1) { new MichelineInt(MemoSize) };
        }

        protected override IMicheline MapValue(object value)
        {
            switch (value)
            {
                case byte[] bytes:
                    return new MichelineBytes(bytes);
                case string str:
                    if (!Hex.TryParse(str, out var b1))
                        throw MapFailedException($"invalid hex string");
                    return new MichelineBytes(b1);
                case JsonElement json when json.ValueKind == JsonValueKind.String:
                    if (!Hex.TryParse(json.GetString(), out var b2))
                        throw MapFailedException($"invalid hex string");
                    return new MichelineBytes(b2);
                default:
                    throw MapFailedException("invalid value");
            }
        }
    }
}
