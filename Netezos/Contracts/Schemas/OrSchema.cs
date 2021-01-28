using System;
using System.Collections.Generic;
using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class OrSchema : Schema
    {
        public override PrimType Prim => PrimType.or;

        public Schema Left { get; }
        public Schema Right { get; }

        public OrSchema(MichelinePrim micheline) : base(micheline)
        {
            if (micheline.Args?.Count != 2
                || !(micheline.Args[0] is MichelinePrim left)
                || !(micheline.Args[1] is MichelinePrim right))
                throw new FormatException($"Invalid {Prim} schema format");

            Left = Create(left);
            Right = Create(right);
        }

        internal override void WriteValue(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();

            foreach (var (path, child) in ChildrenPaths())
            {
                writer.WritePropertyName($"{path}:{child.Signature}");
                child.WriteValue(writer);
            }

            writer.WriteEndObject();
        }

        internal override void WriteValue(Utf8JsonWriter writer, IMicheline value)
        {
            var (endSchema, endValue, endPath) = JumpToEnd(this, value);
            var path = endSchema.Field ?? endSchema.Type ?? endPath;
            
            writer.WriteStartObject();

            writer.WritePropertyName(path);
            endSchema.WriteValue(writer, endValue);

            writer.WriteEndObject();
        }

        protected override List<IMicheline> GetArgs()
        {
            return new List<IMicheline>(2) { Left.ToMicheline(), Right.ToMicheline() };
        }

        (Schema, IMicheline, string) JumpToEnd(OrSchema or, IMicheline value, string path = "")
        {
            var currentSchema = (Schema)or;
            var currentValue = value;
            var currentPath = path;

            while (currentSchema is OrSchema currentOr)
            {
                if (!(currentValue is MichelinePrim prim) || prim.Args?.Count != 1)
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

        IEnumerable<(string, Schema)> ChildrenPaths(string path = "")
        {
            if (Left is OrSchema leftOr)
            {
                foreach (var child in leftOr.ChildrenPaths(path + "L"))
                    yield return child;
            }
            else
            {
                yield return (Left.Field ?? Left.Type ?? path + "L", Left);
            }

            if (Right is OrSchema rightOr)
            {
                foreach (var child in rightOr.ChildrenPaths(path + "R"))
                    yield return child;
            }
            else
            {
                yield return (Right.Field ?? Right.Type ?? path + "R", Right);
            }
        }
    }
}
