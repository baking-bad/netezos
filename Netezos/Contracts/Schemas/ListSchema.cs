using System;
using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class ListSchema : Schema
    {
        public override PrimType Prim => PrimType.list;

        public override string Name => Field ?? Type
            ?? Item.Field ?? Item.Type
            ?? Prim.ToString();

        public Schema Item { get; }

        public ListSchema(MichelinePrim micheline) : base(micheline)
        {
            if (micheline.Args?.Count != 1 || !(micheline.Args[0] is MichelinePrim item))
                throw new FormatException($"Invalid {Prim} schema format");

            Item = Create(item);
        }

        internal override void WriteValue(Utf8JsonWriter writer)
        {
            writer.WriteStartArray();
            Item.WriteValue(writer);
            writer.WriteEndArray();
        }

        internal override void WriteValue(Utf8JsonWriter writer, IMicheline value)
        {
            if (value is MichelineArray micheArray)
            {
                writer.WriteStartArray();

                foreach (var item in micheArray)
                    Item.WriteValue(writer, item);

                writer.WriteEndArray();
            }
            else
            {
                throw FormatException(value);
            }
        }
    }
}
