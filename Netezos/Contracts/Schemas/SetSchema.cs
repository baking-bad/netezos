using System.Collections;
using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class SetSchema : Schema
    {
        public override PrimType Prim => PrimType.set;

        public override string Name => (Annot ?? Item.Annot ?? Prim.ToString()) + Suffix;

        public override string Signature => $"set:{Item.Signature}";

        public Schema Item { get; }

        public SetSchema(MichelinePrim micheline) : base(micheline)
        {
            if (micheline.Args?.Count != 1 || micheline.Args[0] is not MichelinePrim item)
                throw new FormatException($"Invalid {Prim} schema format");

            Item = Create(item);
        }

        internal override TreeView GetTreeView(TreeView? parent, IMicheline value, string? name = null, Schema? schema = null)
        {
            if (value is not MichelineArray micheArray)
                throw FormatException(value);

            var treeView = base.GetTreeView(parent, value, name, schema);

            treeView.Children = new List<TreeView>(micheArray.Count);
            for (int i = 0; i < micheArray.Count; i++)
                treeView.Children.Add(Item.GetTreeView(treeView, micheArray[i], i.ToString()));

            return treeView;
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

        internal override void WriteJsonSchema(Utf8JsonWriter writer)
        {
            writer.WriteString("type", "array");

            writer.WriteStartObject("items");
            Item.WriteJsonSchema(writer);
            writer.WriteEndObject();

            writer.WriteString("$comment", Prim.ToString());
        }

        protected override List<IMicheline> GetArgs()
        {
            return new List<IMicheline>(1) { Item.ToMicheline() };
        }

        protected override IMicheline MapValue(object? value)
        {
            switch (value)
            {
                case IEnumerable e:
                    var arr1 = new MichelineArray();
                    foreach (var item in e)
                        arr1.Add(Item.MapObject(item, true));
                    return arr1;

                case JsonElement json when json.ValueKind == JsonValueKind.Array:
                    var arr2 = new MichelineArray();
                    foreach (var item in json.EnumerateArray())
                        arr2.Add(Item.MapObject(item, true));
                    return arr2;

                default:
                    throw MapFailedException("invalid value");
            }
        }

        public override IMicheline Optimize(IMicheline value)
        {
            if (value is MichelineArray micheArray)
            {
                var res = new MichelineArray(micheArray.Count);
                foreach (var item in micheArray)
                    res.Add(Item.Optimize(item));
                return res;
            }
            return value;
        }
    }
}
