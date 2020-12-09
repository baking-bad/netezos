using System;
using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class MapSchema : Schema
    {
        public override PrimType Prim => PrimType.map;

        public override string Name => Field ?? Type
            ?? Value.Field ?? Value.Type
            ?? Prim.ToString();

        public Schema Key { get; }
        public Schema Value { get; }

        public MapSchema(MichelinePrim micheline) : base(micheline)
        {
            if (micheline.Args?.Count != 2
                || !(micheline.Args[0] is MichelinePrim key)
                || !(micheline.Args[1] is MichelinePrim value))
                throw new FormatException($"Invalid {Prim} schema format");

            Key = Create(key);
            Value = Create(value);
        }

        internal override void WriteValue(Utf8JsonWriter writer)
        {
            if (Key is IFlat)
            {
                writer.WriteStartObject();
                writer.WritePropertyName(Prim.ToString());
                Value.WriteValue(writer);
                writer.WriteEndObject();
            }
            else
            {
                writer.WriteStartArray();
                writer.WriteStartObject();
                writer.WritePropertyName("key");
                Key.WriteValue(writer);
                writer.WritePropertyName("value");
                Value.WriteValue(writer);
                writer.WriteEndObject();
                writer.WriteEndArray();
            }
        }

        internal override void WriteValue(Utf8JsonWriter writer, IMicheline value)
        {
            if (value is MichelineArray micheArray)
            {
                if (Key is IFlat key)
                    WriteMap(writer, key, micheArray);
                else
                    WriteKeyValues(writer, micheArray);
            }
            else
            {
                throw FormatException(value);
            }
        }

        void WriteMap(Utf8JsonWriter writer, IFlat key, MichelineArray items)
        {
            writer.WriteStartObject();

            foreach (var item in items)
            {
                if (!(item is MichelinePrim elt) || elt.Prim != PrimType.Elt || elt.Args?.Count != 2)
                    throw new FormatException($"Invalid map item {(item as MichelinePrim)?.Prim.ToString() ?? item.Type.ToString()}");

                writer.WritePropertyName(key.Flatten(elt.Args[0]));
                Value.WriteValue(writer, elt.Args[1]);
            }

            writer.WriteEndObject();
        }

        void WriteKeyValues(Utf8JsonWriter writer, MichelineArray items)
        {
            writer.WriteStartArray();

            foreach (var item in items)
            {
                writer.WriteStartObject();

                if (!(item is MichelinePrim elt) || elt.Prim != PrimType.Elt || elt.Args?.Count != 2)
                    throw new FormatException($"Invalid map item {(item as MichelinePrim)?.Prim.ToString() ?? item.Type.ToString()}");

                writer.WritePropertyName("key");
                Key.WriteValue(writer, elt.Args[0]);

                writer.WritePropertyName("value");
                Value.WriteValue(writer, elt.Args[1]);

                writer.WriteEndObject();
            }

            writer.WriteEndArray();
        }
    }
}
