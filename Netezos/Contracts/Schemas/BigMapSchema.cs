using System.Collections;
using System.Text.Json;
using Netezos.Encoding;
using Netezos.Forging;
using Netezos.Utils;

namespace Netezos.Contracts
{
    public sealed class BigMapSchema : Schema
    {
        public override PrimType Prim => PrimType.big_map;

        public override string Name => (Annot ?? Value.Annot ?? Prim.ToString()) + Suffix;

        public override string Signature =>
            $"{(Key is IFlat ? "big_map_flat" : "big_map")}:{Key.Signature}:{Value.Signature}";

        public Schema Key { get; }
        public Schema Value { get; }

        public BigMapSchema(MichelinePrim micheline) : base(micheline)
        {
            if (micheline.Args?.Count != 2
                || micheline.Args[0] is not MichelinePrim key
                || micheline.Args[1] is not MichelinePrim value)
                throw new FormatException($"Invalid {Prim} schema format");

            Key = Create(key);
            Value = Create(value);
        }

        internal override TreeView GetTreeView(TreeView? parent, IMicheline value, string? name = null, Schema? schema = null)
        {
            if (value is MichelineInt)
                return base.GetTreeView(parent, value, name, schema);

            if (value is not MichelineArray micheArray)
                throw FormatException(value);

            var treeView = base.GetTreeView(parent, value, name, schema);
            treeView.Children = new List<TreeView>(micheArray.Count);

            if (Key is IFlat key)
            {
                foreach (var item in micheArray)
                {
                    if (item is not MichelinePrim elt || elt.Prim != PrimType.Elt || elt.Args?.Count != 2)
                        throw new FormatException($"Invalid map item {(item as MichelinePrim)?.Prim.ToString() ?? item.Type.ToString()}");

                    treeView.Children.Add(Value.GetTreeView(treeView, elt.Args[1], key.Flatten(elt.Args[0])));
                }
            }
            else
            {
                foreach (var item in micheArray)
                {
                    if (item is not MichelinePrim elt || elt.Prim != PrimType.Elt || elt.Args?.Count != 2)
                        throw new FormatException($"Invalid map item {(item as MichelinePrim)?.Prim.ToString() ?? item.Type.ToString()}");

                    var keyStr = Key.Humanize(elt.Args[0], new JsonWriterOptions { Indented = false });
                    treeView.Children.Add(Value.GetTreeView(treeView, elt.Args[1], keyStr));
                }
            }

            return treeView;
        }

        internal override void WriteValue(Utf8JsonWriter writer)
        {
            if (Key is IFlat)
            {
                writer.WriteStartObject();
                writer.WritePropertyName(Key.Prim.ToString());
                Value.WriteValue(writer);
                writer.WriteEndObject();
            }
            else
            {
                writer.WriteStartArray();
                writer.WriteStartObject();
                writer.WritePropertyName($"key:{Key.Signature}");
                Key.WriteValue(writer);
                writer.WritePropertyName($"value:{Value.Signature}");
                Value.WriteValue(writer);
                writer.WriteEndObject();
                writer.WriteEndArray();
            }
        }

        internal override void WriteValue(Utf8JsonWriter writer, IMicheline value)
        {
            if (value is MichelineInt micheInt)
            {
                writer.WriteNumberValue((long)micheInt.Value);
            }
            else if (value is MichelineArray micheArray)
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
                if (item is not MichelinePrim elt || elt.Prim != PrimType.Elt || elt.Args?.Count != 2)
                    throw new FormatException($"Invalid big_map item {(item as MichelinePrim)?.Prim.ToString() ?? item.Type.ToString()}");

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

                if (item is not MichelinePrim elt || elt.Prim != PrimType.Elt || elt.Args?.Count != 2)
                    throw new FormatException($"Invalid big_map item {(item as MichelinePrim)?.Prim.ToString() ?? item.Type.ToString()}");

                writer.WritePropertyName("key");
                Key.WriteValue(writer, elt.Args[0]);

                writer.WritePropertyName("value");
                Value.WriteValue(writer, elt.Args[1]);

                writer.WriteEndObject();
            }

            writer.WriteEndArray();
        }

        internal override void WriteJsonSchema(Utf8JsonWriter writer)
        {
            writer.WriteStartArray("oneOf");
            {
                writer.WriteStartObject();
                writer.WriteString("type", "integer");
                writer.WriteString("$comment", "int");
                writer.WriteEndObject();

                writer.WriteStartObject();
                if (Key is IFlat)
                {
                    writer.WriteString("type", "object");

                    writer.WriteStartObject("propertyNames");
                    Key.WriteJsonSchema(writer);
                    writer.WriteEndObject();

                    writer.WriteStartObject("additionalProperties");
                    Value.WriteJsonSchema(writer);
                    writer.WriteEndObject();
                }
                else
                {
                    writer.WriteString("type", "array");
                    writer.WriteStartObject("items");
                    {
                        writer.WriteString("type", "object");

                        writer.WriteStartObject("properties");
                        {
                            writer.WriteStartObject("key");
                            Key.WriteJsonSchema(writer);
                            writer.WriteEndObject();

                            writer.WriteStartObject("value");
                            Value.WriteJsonSchema(writer);
                            writer.WriteEndObject();
                        }
                        writer.WriteEndObject();

                        writer.WriteStartArray("required");
                        writer.WriteStringValue("key");
                        writer.WriteStringValue("value");
                        writer.WriteEndArray();

                        writer.WriteBoolean("additionalProperties", false);
                    }
                    writer.WriteEndObject();
                }
                writer.WriteEndObject();
            }
            writer.WriteEndArray();

            if (Key is IFlat)
            {
                writer.WriteStartObject("default");
                writer.WriteEndObject();
            }
            else
            {
                writer.WriteStartArray("default");
                writer.WriteEndArray();
            }

            writer.WriteString("$comment", Prim.ToString());
        }

        protected override List<IMicheline> GetArgs()
        {
            return new List<IMicheline>(2) { Key.ToMicheline(), Value.ToMicheline() };
        }

        protected override IMicheline MapValue(object value)
        {
            switch (value)
            {
                case IEnumerable e:
                    var arr1 = new MichelineArray();
                    foreach (var item in e)
                    {
                        var type = item.GetType();
                        var keyProp = type.GetProperty("Key") ?? type.GetProperty("key")
                            ?? throw MapFailedException("missed 'key' property");
                        var valueProp = type.GetProperty("Value") ?? type.GetProperty("value")
                            ?? throw MapFailedException("missed 'value' property");

                        arr1.Add(new MichelinePrim
                        {
                            Prim = PrimType.Elt,
                            Args = new List<IMicheline>(2)
                            {
                                Key.MapObject(keyProp.GetValue(item), true),
                                Value.MapObject(valueProp.GetValue(item), true)
                            }
                        });
                    }
                    return arr1;

                case JsonElement json when json.ValueKind == JsonValueKind.Object:
                    var arr2 = new MichelineArray();
                    foreach (var item in json.EnumerateObject())
                    {
                        arr2.Add(new MichelinePrim
                        {
                            Prim = PrimType.Elt,
                            Args = new List<IMicheline>(2)
                            {
                                Key.MapObject(item.Name, true),
                                Value.MapObject(item.Value, true)
                            }
                        });
                    }
                    return arr2;

                case JsonElement json when json.ValueKind == JsonValueKind.Array:
                    var arr3 = new MichelineArray();
                    foreach (var item in json.EnumerateArray())
                    {
                        if (!item.TryGetProperty("key", out var key) && !item.TryGetProperty("Key", out key))
                            throw MapFailedException("missed 'key' property");
                        if (!item.TryGetProperty("value", out var val) && !item.TryGetProperty("Value", out val))
                            throw MapFailedException("missed 'value' property");

                        arr3.Add(new MichelinePrim
                        {
                            Prim = PrimType.Elt,
                            Args = new List<IMicheline>(2)
                            {
                                Key.MapObject(key, true),
                                Value.MapObject(val, true)
                            }
                        });
                    }
                    return arr3;

                default:
                    throw MapFailedException("invalid value");
            }
        }

        public override IMicheline Optimize(IMicheline value)
        {
            if (value is MichelineArray micheArray)
            {
                foreach (var item in micheArray)
                {
                    if (item is MichelinePrim { Prim: PrimType.Elt, Args.Count: 2 } elt)
                    {
                        elt.Args[0] = Key.Optimize(elt.Args[0]);
                        elt.Args[1] = Value.Optimize(elt.Args[1]);
                    }
                }
            }

            return value;
        }

        public string GetKeyHash(IMicheline key)
        {
            var optimized = Key.Optimize(Micheline.FromBytes(key.ToBytes()));
            var packed = new byte[] { 5 }.Concat(LocalForge.ForgeMicheline(optimized));
            var hash = Blake2b.GetDigest(packed);

            return Base58.Convert(hash, Prefix.expr);
        }
    }
}
