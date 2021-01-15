﻿using System;
using System.Collections.Generic;
using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class TicketSchema : Schema
    {
        public override PrimType Prim => PrimType.ticket;

        public Schema Data { get; }

        public TicketSchema(MichelinePrim micheline) : base(micheline)
        {
            if (micheline.Args?.Count != 1 || !(micheline.Args[0] is MichelinePrim type))
                throw new FormatException($"Invalid {Prim} schema format");

            var data = new MichelinePrim
            {
                Prim = PrimType.pair,
                Args = new List<IMicheline>(2)
                {
                    new MichelinePrim
                    {
                        Prim = PrimType.address
                    },
                    new MichelinePrim
                    {
                        Prim = PrimType.pair,
                        Args = new List<IMicheline>(2)
                        {
                            type,
                            new MichelinePrim
                            {
                                Prim = PrimType.nat
                            }
                        }
                    }
                }
            };

            Data = Create(data);
        }

        internal override void WriteValue(Utf8JsonWriter writer)
        {
            Data.WriteValue(writer);
        }

        internal override void WriteValue(Utf8JsonWriter writer, IMicheline value)
        {
            if (!(value is MichelinePrim prim) || prim.Prim != PrimType.Pair)
                throw FormatException(value);

            Data.WriteValue(writer, prim);
        }
    }
}
