﻿using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class OrSchema : Schema
    {
        public override PrimType Prim => PrimType.or;

        public Schema Left { get; }
        public Schema Right { get; }

        public OrSchema(MichelinePrim micheline, bool nested = false) : base(micheline)
        {
            if (micheline.Args?.Count != 2
                || micheline.Args[0] is not MichelinePrim left
                || micheline.Args[1] is not MichelinePrim right)
                throw new FormatException($"Invalid {Prim} schema format");

            Left = left.Prim == PrimType.or
                ? new OrSchema(left, true)
                : Create(left);

            Right = right.Prim == PrimType.or
                ? new OrSchema(right, true)
                : Create(right);

            if (!nested)
            {
                var fields = new Dictionary<string, int>();
                var children = Children();
                foreach (var child in children.Where(x => x.Name != null))
                {
                    var name = child.Name!;
                    if (!fields.TryAdd(name, 0))
                        child.Index = ++fields[name];
                }
                foreach (var kv in fields.Where(x => x.Value > 0))
                    children.First(x => x.Name == kv.Key).Index = 0;
            }
        }

        internal override TreeView GetTreeView(TreeView? parent, IMicheline value, string? name = null, Schema? schema = null)
        {
            var (endSchema, endValue, endPath) = JumpToEnd(this, value);
            var path = endSchema.Annot != null
                ? endSchema.Annot + endSchema.Suffix
                : endPath;

            var treeView = base.GetTreeView(parent, value, name, schema);
            treeView.Children =
            [
                endSchema.GetTreeView(treeView, endValue, path)
            ];

            return treeView;
        }

        protected override IMicheline MapValue(object? value)
        {
            if (value is JsonElement json && json.ValueKind == JsonValueKind.Object)
            {
                foreach (var (path, pathName, child) in ChildrenPaths())
                {
                    if (json.TryGetProperty(pathName, out var pathValue))
                        return MapOr(path, pathValue, child);
                }
            }
            else
            {
                var type = value?.GetType()
                    ?? throw MapFailedException("value cannot be null");

                foreach (var (path, pathName, child) in ChildrenPaths())
                {
                    var pathValue = type.GetProperty(pathName)?.GetValue(value);
                    if (pathValue != null)
                        return MapOr(path, pathValue, child);
                }
            }

            throw MapFailedException("no paths matched");
        }

        static MichelinePrim MapOr(string path, object value, Schema child)
        {
            var res = new MichelinePrim
            {
                Prim = path[0] == 'L' ? PrimType.Left : PrimType.Right,
                Args = new List<IMicheline>(1)
            };

            var currPath = res;
            for (int i = 1; i < path.Length; i++)
            {
                var or = new MichelinePrim
                {
                    Prim = path[i] == 'L' ? PrimType.Left : PrimType.Right,
                    Args = new List<IMicheline>(1)
                };
                currPath.Args.Add(or);
                currPath = or;
            }
            currPath.Args.Add(child.MapObject(value, true));

            return res;
        }

        internal override void WriteValue(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();

            foreach (var (_, pathName, child) in ChildrenPaths())
            {
                writer.WritePropertyName($"{pathName}:{child.Signature}");
                child.WriteValue(writer);
            }

            writer.WriteEndObject();
        }

        internal override void WriteValue(Utf8JsonWriter writer, IMicheline value)
        {
            var (endSchema, endValue, endPath) = JumpToEnd(this, value);
            var path = endSchema.Annot != null
                ? endSchema.Annot + endSchema.Suffix
                : endPath;

            writer.WriteStartObject();

            writer.WritePropertyName(path);
            endSchema.WriteValue(writer, endValue);

            writer.WriteEndObject();
        }

        internal override void WriteJsonSchema(Utf8JsonWriter writer)
        {
            writer.WriteStartArray("oneOf");
            foreach (var (_, pathName, child) in ChildrenPaths())
            {
                writer.WriteStartObject();
                {
                    writer.WriteString("type", "object");

                    writer.WriteStartObject("properties");
                    {
                        writer.WriteStartObject(pathName);
                        child.WriteJsonSchema(writer);
                        writer.WriteEndObject();
                    }
                    writer.WriteEndObject();

                    writer.WriteStartArray("required");
                    writer.WriteStringValue(pathName);
                    writer.WriteEndArray();

                    writer.WriteBoolean("additionalProperties", false);
                }
                writer.WriteEndObject();
            }
            writer.WriteEndArray();

            writer.WriteString("$comment", Prim.ToString());
        }

        protected override List<IMicheline> GetArgs()
        {
            return [Left.ToMicheline(), Right.ToMicheline()];
        }

        (Schema, IMicheline, string) JumpToEnd(OrSchema or, IMicheline value, string path = "")
        {
            var currentSchema = (Schema)or;
            var currentValue = value;
            var currentPath = path;

            while (currentSchema is OrSchema currentOr)
            {
                if (currentValue is not MichelinePrim { Args.Count: 1 } prim)
                    throw FormatException(value);

                if (prim.Prim == PrimType.Left)
                {
                    currentSchema = currentOr.Left;
                    currentValue = prim.Args[0];
                    currentPath += "L";
                }
                else if (prim.Prim == PrimType.Right)
                {
                    currentSchema = currentOr.Right;
                    currentValue = prim.Args[0];
                    currentPath += "R";
                }
                else
                {
                    throw FormatException(value);
                }
            }

            return (currentSchema, currentValue, currentPath);
        }

        public IEnumerable<Schema> Children()
        {
            if (Left is OrSchema leftOr)
            {
                foreach (var child in leftOr.Children())
                    yield return child;
            }
            else
            {
                yield return Left;
            }

            if (Right is OrSchema rightOr)
            {
                foreach (var child in rightOr.Children())
                    yield return child;
            }
            else
            {
                yield return Right;
            }
        }

        IEnumerable<(string, string, Schema)> ChildrenPaths(string path = "")
        {
            if (Left is OrSchema leftOr)
            {
                foreach (var child in leftOr.ChildrenPaths(path + "L"))
                    yield return child;
            }
            else
            {
                var curPath = path + "L";
                var curPathName = Left.Annot != null
                    ? Left.Annot + Left.Suffix
                    : curPath;

                yield return (curPath, curPathName, Left);
            }

            if (Right is OrSchema rightOr)
            {
                foreach (var child in rightOr.ChildrenPaths(path + "R"))
                    yield return child;
            }
            else
            {
                var curPath = path + "R";
                var curPathName = Right.Annot != null
                    ? Right.Annot + Right.Suffix
                    : curPath;

                yield return (curPath, curPathName, Right);
            }
        }

        public override IMicheline Optimize(IMicheline value)
        {
            if (value is MichelinePrim { Args.Count: 1 } prim)
            {
                if (prim.Prim == PrimType.Left)
                    prim.Args[0] = Left.Optimize(prim.Args[0]);

                if (prim.Prim == PrimType.Right)
                    prim.Args[0] = Right.Optimize(prim.Args[0]);
            }

            return value;
        }
    }
}
