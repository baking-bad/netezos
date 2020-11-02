using System;
using System.Collections.Generic;
using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class PairSchema : Schema
    {
        public override PrimType Prim => PrimType.pair;

        public override string Name => Field ?? Type;

        public Schema Left { get; }
        public Schema Right { get; }
        public PairKind Kind { get; }

        public PairSchema(MichelinePrim micheline, bool nested = false) : base(micheline)
        {
            if (micheline.Args?.Count != 2
                || !(micheline.Args[0] is MichelinePrim left)
                || !(micheline.Args[1] is MichelinePrim right))
                throw new FormatException($"Invalid {Prim} schema format");

            Left = left.Prim == PrimType.pair
                ? new PairSchema(left, true)
                : Create(left);

            Right = right.Prim == PrimType.pair
                ? new PairSchema(right, true)
                : Create(right);

            if (nested && Name == null)
            {
                Kind = PairKind.Nested;
            }
            else
            {
                Kind = PairKind.Object;
                var fields = new HashSet<string>();
                foreach (var child in Children())
                {
                    var name = child.Name;
                    if (fields.Contains(name))
                    {
                        Kind = PairKind.Tuple;
                        break;
                    }
                    fields.Add(name);
                }
            }
        }

        internal override void WriteProperty(Utf8JsonWriter writer)
        {
            if (Kind == PairKind.Object)
            {
                writer.WritePropertyName(Name);
                writer.WriteStartObject();

                Left.WriteProperty(writer);
                Right.WriteProperty(writer);

                writer.WriteEndObject();
            }
            else if (Kind == PairKind.Tuple)
            {
                writer.WritePropertyName(Name);
                writer.WriteStartArray();

                Left.WriteValue(writer);
                Right.WriteValue(writer);

                writer.WriteEndArray();
            }
            else
            {
                Left.WriteProperty(writer);
                Right.WriteProperty(writer);
            }
        }

        internal override void WriteProperty(Utf8JsonWriter writer, IMicheline value)
        {
            if (!(value is MichelinePrim pair) || pair.Prim != PrimType.Pair)
                throw FormatException(value);

            if (pair.Args?.Count != 2)
                throw new FormatException($"Invalid 'pair' prim args count");

            if (Kind == PairKind.Object)
            {
                writer.WritePropertyName(Name);
                writer.WriteStartObject();

                Left.WriteProperty(writer, pair.Args[0]);
                Right.WriteProperty(writer, pair.Args[1]);

                writer.WriteEndObject();
            }
            else if (Kind == PairKind.Tuple)
            {
                writer.WritePropertyName(Name);
                writer.WriteStartArray();

                Left.WriteValue(writer, pair.Args[0]);
                Right.WriteValue(writer, pair.Args[1]);

                writer.WriteEndArray();
            }
            else
            {
                Left.WriteProperty(writer, pair.Args[0]);
                Right.WriteProperty(writer, pair.Args[1]);
            }
        }

        internal override void WriteValue(Utf8JsonWriter writer)
        {
            if (Kind == PairKind.Object)
            {
                writer.WriteStartObject();

                Left.WriteProperty(writer);
                Right.WriteProperty(writer);

                writer.WriteEndObject();
            }
            else if (Kind == PairKind.Tuple)
            {
                writer.WriteStartArray();

                Left.WriteValue(writer);
                Right.WriteValue(writer);

                writer.WriteEndArray();
            }
            else
            {
                Left.WriteValue(writer);
                Right.WriteValue(writer);
            }
        }

        internal override void WriteValue(Utf8JsonWriter writer, IMicheline value)
        {
            if (!(value is MichelinePrim pair) || pair.Prim != PrimType.Pair)
                throw FormatException(value);

            if (pair.Args?.Count != 2)
                throw new FormatException($"Invalid 'pair' prim args count");

            if (Kind == PairKind.Object)
            {
                writer.WriteStartObject();

                Left.WriteProperty(writer, pair.Args[0]);
                Right.WriteProperty(writer, pair.Args[1]);

                writer.WriteEndObject();
            }
            else if (Kind == PairKind.Tuple)
            {
                writer.WriteStartArray();

                Left.WriteValue(writer, pair.Args[0]);
                Right.WriteValue(writer, pair.Args[1]);

                writer.WriteEndArray();
            }
            else
            {
                Left.WriteValue(writer, pair.Args[0]);
                Right.WriteValue(writer, pair.Args[1]);
            }
        }

        IEnumerable<Schema> Children()
        {
            if (Left is PairSchema leftPair && Left.Name == null)
            {
                foreach (var child in leftPair.Children())
                    yield return child;
            }
            else
            {
                yield return Left;
            }

            if (Right is PairSchema rightPair && Right.Name == null)
            {
                foreach (var child in rightPair.Children())
                    yield return child;
            }
            else
            {
                yield return Right;
            }
        }
    }

    public enum PairKind
    {
        Nested,
        Object,
        Tuple
    }
}
