using System.Collections;
using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class PairSchema : Schema
    {
        public override PrimType Prim => PrimType.pair;

        public override string? Name => Annot;

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
                || micheline.Args[0] is not MichelinePrim left
                || micheline.Args[1] is not MichelinePrim right)
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
                    var name = child.Name!;
                    if (fields.ContainsKey(name))
                        child.Index = ++fields[name];
                    else
                        fields.Add(name, 0);
                }
                foreach (var kv in fields.Where(x => x.Value > 0))
                    children.First(x => x.Name == kv.Key).Index = 0;
            }
        }

        internal override TreeView GetTreeView(TreeView? parent, IMicheline value, string? name = null, Schema? schema = null)
        {
            var treeView = base.GetTreeView(parent, value, name, schema);

            treeView.Children = Children(value)
                .Select(x => x.Item1.GetTreeView(treeView, x.Item2))
                .ToList();

            return treeView;
        }

        public override IMicheline MapObject(object? obj, bool isValue = false)
        {
            if (Kind == PairKind.Object)
            {
                if (obj is IEnumerable e)
                    return MapPair(e.GetEnumerator());

                if (isValue)
                    return MapPair(obj);

                switch (obj)
                {
                    case IEnumerator enumerator:
                        if (!enumerator.MoveNext())
                            throw MapFailedException($"enumerable is over");
                        return MapPair(enumerator.Current);
                    case JsonElement json:
                        if (Name == null)
                            return MapPair(json);
                        if (!json.TryGetProperty(Name, out var jsonProp))
                            throw MapFailedException($"no such property");
                        return MapPair(jsonProp);
                    default:
                        if (Name == null)
                            return MapPair(obj);
                        var prop = obj?.GetType()?.GetProperty(Name)
                            ?? throw MapFailedException($"no such property");
                        return MapPair(prop.GetValue(obj));
                }
            }
            else
            {
                return MapPair(obj);
            }
        }

        IMicheline MapPair(object? value) => new MichelinePrim
        {
            Prim = PrimType.Pair,
            Args = new List<IMicheline>(2)
            {
                Left.MapObject(value),
                Right.MapObject(value)
            }
        };

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
            value = Uncomb(value);

            if (value is not MichelinePrim { Prim: PrimType.Pair } pair)
                throw FormatException(value);

            if (pair.Args?.Count != 2)
                throw new FormatException($"Invalid 'pair' prim args count");

            if (Kind == PairKind.Object)
            {
                writer.WritePropertyName(Name!);
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
            value = Uncomb(value);

            if (value is not MichelinePrim { Prim: PrimType.Pair } pair)
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

        internal void WriteJsonSchema(Utf8JsonWriter writer, string comment)
        {
            writer.WriteString("type", "object");

            writer.WriteStartObject("properties");
            foreach (var child in Children())
            {
                writer.WriteStartObject(child.Name!);
                child.WriteJsonSchema(writer);
                writer.WriteEndObject();
            }
            writer.WriteEndObject();

            writer.WriteStartArray("required");
            foreach (var child in Children())
                writer.WriteStringValue(child.Name);
            writer.WriteEndArray();

            writer.WriteBoolean("additionalProperties", false);
            writer.WriteString("$comment", comment);
        }

        internal override void WriteJsonSchema(Utf8JsonWriter writer)
        {
            WriteJsonSchema(writer, Prim.ToString());
        }

        protected override List<IMicheline> GetArgs()
        {
            return new List<IMicheline>(2) { Left.ToMicheline(), Right.ToMicheline() };
        }

        IMicheline Uncomb(IMicheline value)
        {
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
            return value;
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

        IEnumerable<(Schema, IMicheline)> Children(IMicheline value)
        {
            value = Uncomb(value);

            if (value is not MichelinePrim { Prim: PrimType.Pair } pair)
                throw FormatException(value);

            if (pair.Args?.Count != 2)
                throw new FormatException($"Invalid 'pair' prim args count");

            if (Left is PairSchema leftPair && Left.Name == null)
            {
                foreach (var child in leftPair.Children(pair.Args[0]))
                    yield return child;
            }
            else
            {
                yield return (Left, pair.Args[0]);
            }

            if (Right is PairSchema rightPair && Right.Name == null)
            {
                foreach (var child in rightPair.Children(pair.Args[1]))
                    yield return child;
            }
            else
            {
                yield return (Right, pair.Args[1]);
            }
        }

        public override IMicheline Optimize(IMicheline value)
        {
            value = Uncomb(value);
            if (value is MichelinePrim { Args.Count: 2 } prim)
            {
                prim.Args[0] = Left.Optimize(prim.Args[0]);
                prim.Args[1] = Right.Optimize(prim.Args[1]);
            }
            return value;
        }
    }

    public enum PairKind
    {
        Nested,
        Object
    }
}
