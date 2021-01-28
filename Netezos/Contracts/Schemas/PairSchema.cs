using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class PairSchema : Schema
    {
        public override PrimType Prim => PrimType.pair;

        public override string Name => Field ?? Type;

        public override string Signature => "object";

        public Schema Left { get; }
        public Schema Right { get; }
        public PairKind Kind { get; }

        public PairSchema(MichelinePrim micheline, bool nested = false) : base(micheline)
        {
            #region deoptimize
            if (micheline.Args?.Count > 2)
            {
                micheline = new MichelinePrim
                {
                    Prim = micheline.Prim,
                    Annots = micheline.Annots,
                    Args = new List<IMicheline>(2)
                    {
                        micheline.Args[0],
                        new MichelinePrim
                        {
                            Prim = PrimType.pair,
                            Args = micheline.Args.Skip(1).ToList()
                        }
                    }
                };
            }
            #endregion

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
                var fields = new Dictionary<string, int>();
                var children = Children();
                foreach (var child in children)
                {
                    var name = child.Name;
                    if (fields.ContainsKey(name))
                        child._Suffix = ++fields[name];
                    else
                        fields.Add(name, 0);
                }
                foreach (var kv in fields.Where(x => x.Value > 0))
                    children.First(x => x.Name == kv.Key)._Suffix = 0;
            }
        }

        internal override void WriteProperty(Utf8JsonWriter writer)
        {
            if (Kind == PairKind.Object)
            {
                writer.WritePropertyName($"{Name}:{Signature}");
                writer.WriteStartObject();

                Left.WriteProperty(writer);
                Right.WriteProperty(writer);

                writer.WriteEndObject();
            }
            else
            {
                Left.WriteProperty(writer);
                Right.WriteProperty(writer);
            }
        }

        internal override void WriteProperty(Utf8JsonWriter writer, IMicheline value)
        {
            #region deoptimize
            if (value is MichelineArray array)
            {
                value = new MichelinePrim
                {
                    Prim = PrimType.Pair,
                    Args = array
                };
            }
            if (value is MichelinePrim p && p.Prim == PrimType.Pair && p.Args?.Count > 2)
            {
                value = new MichelinePrim
                {
                    Prim = p.Prim,
                    Annots = p.Annots,
                    Args = new List<IMicheline>(2)
                    {
                        p.Args[0],
                        new MichelinePrim
                        {
                            Prim = PrimType.Pair,
                            Args = p.Args.Skip(1).ToList()
                        }
                    }
                };
            }
            #endregion

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
            else
            {
                Left.WriteValue(writer);
                Right.WriteValue(writer);
            }
        }

        internal override void WriteValue(Utf8JsonWriter writer, IMicheline value)
        {
            #region deoptimize
            if (value is MichelineArray array)
            {
                value = new MichelinePrim
                {
                    Prim = PrimType.Pair,
                    Args = array
                };
            }
            if (value is MichelinePrim p && p.Prim == PrimType.Pair && p.Args?.Count > 2)
            {
                value = new MichelinePrim
                {
                    Prim = p.Prim,
                    Annots = p.Annots,
                    Args = new List<IMicheline>(2)
                    {
                        p.Args[0],
                        new MichelinePrim
                        {
                            Prim = PrimType.Pair,
                            Args = p.Args.Skip(1).ToList()
                        }
                    }
                };
            }
            #endregion

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
            else
            {
                Left.WriteValue(writer, pair.Args[0]);
                Right.WriteValue(writer, pair.Args[1]);
            }
        }

        protected override List<IMicheline> GetArgs()
        {
            return new List<IMicheline>(2) { Left.ToMicheline(), Right.ToMicheline() };
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
        Object
    }
}
