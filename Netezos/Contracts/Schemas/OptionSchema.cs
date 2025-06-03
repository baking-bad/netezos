using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class OptionSchema : Schema
    {
        public override PrimType Prim => PrimType.option;

        public override string Name => (Annot ?? Some.Annot ?? Some.Prim.ToString()) + Suffix;

        public override string Signature => $"?{Some.Signature}";

        public Schema Some { get; }

        public OptionSchema(MichelinePrim micheline) : base(micheline)
        {
            if (micheline.Args?.Count != 1 || micheline.Args[0] is not MichelinePrim some)
                throw new FormatException($"Invalid {Prim} schema format");

            Some = Create(some);
        }

        internal override TreeView GetTreeView(TreeView? parent, IMicheline value, string? name = null, Schema? schema = null)
        {
            if (value is not MichelinePrim prim)
                throw FormatException(value);

            if (prim.Prim == PrimType.None)
            {
                return base.GetTreeView(parent, value, name);
            }
            else if (prim.Prim == PrimType.Some)
            {
                if (prim.Args?.Count != 1)
                    throw new FormatException("Invalid 'Some' prim args count");

                var treeView = base.GetTreeView(parent, value, name);
                treeView.Children = new(1) { Some.GetTreeView(treeView, prim.Args[0], name ?? Name) };
                return treeView;
            }
            else
            {
                throw FormatException(value);
            }

        }

        internal override void WriteValue(Utf8JsonWriter writer)
        {
            Some.WriteValue(writer);
        }

        internal override void WriteValue(Utf8JsonWriter writer, IMicheline value)
        {
            if (value is not MichelinePrim prim)
                throw FormatException(value);

            if (prim.Prim == PrimType.None)
            {
                writer.WriteNullValue();
            }
            else if (prim.Prim == PrimType.Some)
            {
                if (prim.Args?.Count != 1)
                    throw new FormatException("Invalid 'Some' prim args count");

                Some.WriteValue(writer, prim.Args[0]);
            }
            else
            {
                throw FormatException(value);
            }
        }

        internal override void WriteJsonSchema(Utf8JsonWriter writer)
        {
            writer.WriteStartArray("oneOf");
            {
                writer.WriteStartObject();
                Some.WriteJsonSchema(writer);
                writer.WriteEndObject();

                writer.WriteStartObject();
                writer.WriteString("type", "null");
                writer.WriteEndObject();
            }
            writer.WriteEndArray();

            writer.WriteString("$comment", Prim.ToString());
        }

        protected override List<IMicheline> GetArgs()
        {
            return new List<IMicheline>(1) { Some.ToMicheline() };
        }

        protected override IMicheline MapValue(object? value)
        {
            return value == null || value is JsonElement json && json.ValueKind == JsonValueKind.Null
                ? new MichelinePrim
                {
                    Prim = PrimType.None
                }
                : new MichelinePrim
                {
                    Prim = PrimType.Some,
                    Args = new List<IMicheline>(1)
                    {
                        Some.MapObject(value, true)
                    }
                };
        }

        public override IMicheline Optimize(IMicheline value)
        {
            if (value is MichelinePrim { Prim: PrimType.Some, Args.Count: 1 } prim)
            {
                prim.Args[0] = Some.Optimize(prim.Args[0]);
            }

            return value;
        }
    }
}
