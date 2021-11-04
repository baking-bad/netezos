using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Netezos.Encoding.Serialization
{
    public class MichelineConverter : JsonConverter<IMicheline>
    {
        public override IMicheline Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Stack<IMicheline> stack = new();
            IMicheline node, top;
            MichelinePrim prim;
            string prop;

        start:
            switch (reader.TokenType)
            {
                case JsonTokenType.StartObject:
                    reader.Read();
                    if (reader.TokenType != JsonTokenType.PropertyName)
                        throw new FormatException("Empty Micheline node");
                    prop = reader.GetString();
                    reader.Read();
                    switch (prop)
                    {
                        case "prim":
                            stack.Push(new MichelinePrim { Prim = PrimTypeConverter.ParsePrim(reader.GetString()) });
                            reader.Read();
                            goto start;
                        case "args":
                            stack.Push(new MichelinePrim());
                            if (reader.TokenType == JsonTokenType.StartArray)
                            {
                                reader.Read();
                                if (reader.TokenType != JsonTokenType.EndArray)
                                {
                                    stack.Push(new MichelineArray(2));
                                    goto start;
                                }
                            }
                            else if (reader.TokenType != JsonTokenType.Null)
                            {
                                throw new FormatException("Invalid prim args");
                            }
                            reader.Read();
                            goto start;
                        case "annots":
                            List<IAnnotation> annots = null;
                            if (reader.TokenType == JsonTokenType.StartArray)
                            {
                                reader.Read();
                                if (reader.TokenType == JsonTokenType.String)
                                {
                                    annots = new(2);
                                    annots.Add(AnnotationConverter.ParseAnnotation(reader.GetString()));
                                    reader.Read();
                                    while (reader.TokenType == JsonTokenType.String)
                                    {
                                        annots.Add(AnnotationConverter.ParseAnnotation(reader.GetString()));
                                        reader.Read();
                                    }
                                }
                                if (reader.TokenType != JsonTokenType.EndArray)
                                    throw new FormatException("Invalid prim annotation");
                            }
                            else if (reader.TokenType != JsonTokenType.Null)
                            {
                                throw new FormatException("Invalid prim annots");
                            }
                            stack.Push(new MichelinePrim { Annots = annots });
                            reader.Read();
                            goto start;
                        case "bytes":
                            if (reader.TokenType != JsonTokenType.String)
                                throw new FormatException("Invalid Micheline bytes node");
                            node = new MichelineBytes(Hex.Parse(reader.GetString()));
                            break;
                        case "string":
                            if (reader.TokenType != JsonTokenType.String)
                                throw new FormatException("Invalid Micheline string node");
                            node = new MichelineString(reader.GetString());
                            break;
                        case "int":
                            if (reader.TokenType != JsonTokenType.String)
                                throw new FormatException("Invalid Micheline int node");
                            node = new MichelineInt(BigInteger.Parse(reader.GetString()));
                            break;
                        default:
                            throw new FormatException("Invalid Micheline node");
                    }
                    reader.Read();
                    if (reader.TokenType != JsonTokenType.EndObject)
                        throw new FormatException($"Invalid Micheline {node.Type} node");
                    goto endNode;
                case JsonTokenType.PropertyName:
                    prim = (MichelinePrim)stack.Peek();
                    prop = reader.GetString();
                    reader.Read();
                    switch (prop)
                    {
                        case "prim":
                            prim.Prim = PrimTypeConverter.ParsePrim(reader.GetString());
                            reader.Read();
                            goto start;
                        case "args":
                            if (reader.TokenType == JsonTokenType.StartArray)
                            {
                                reader.Read();
                                if (reader.TokenType != JsonTokenType.EndArray)
                                {
                                    stack.Push(new MichelineArray(2));
                                    goto start;
                                }
                            }
                            else if (reader.TokenType != JsonTokenType.Null)
                            {
                                throw new FormatException("Invalid prim args");
                            }
                            reader.Read();
                            goto start;
                        case "annots":
                            List<IAnnotation> annots = null;
                            if (reader.TokenType == JsonTokenType.StartArray)
                            {
                                reader.Read();
                                if (reader.TokenType == JsonTokenType.String)
                                {
                                    annots = new(2);
                                    annots.Add(AnnotationConverter.ParseAnnotation(reader.GetString()));
                                    reader.Read();
                                    while (reader.TokenType == JsonTokenType.String)
                                    {
                                        annots.Add(AnnotationConverter.ParseAnnotation(reader.GetString()));
                                        reader.Read();
                                    }
                                }
                                if (reader.TokenType != JsonTokenType.EndArray)
                                    throw new FormatException("Invalid prim annotation");
                            }
                            else if (reader.TokenType != JsonTokenType.Null)
                            {
                                throw new FormatException("Invalid prim annots");
                            }
                            prim.Annots = annots;
                            reader.Read();
                            goto start;
                        default:
                            throw new FormatException();
                    }
                case JsonTokenType.EndObject:
                    node = stack.Pop();
                endNode:
                    if (stack.Count == 0) return node;
                    ((MichelineArray)stack.Peek()).Add(node);
                    reader.Read();
                    goto start;
                case JsonTokenType.StartArray:
                    stack.Push(new MichelineArray());
                    reader.Read();
                    goto start;
                case JsonTokenType.EndArray:
                    node = stack.Pop();
                    if (stack.Count == 0) return node;
                    top = stack.Peek();
                    if (top is MichelinePrim pr)
                        pr.Args = (MichelineArray)node;
                    else
                        ((MichelineArray)top).Add(node);
                    reader.Read();
                    goto start;
                default:
                    throw new FormatException("Invalid Micheline format");
            }
        }

        public override void Write(Utf8JsonWriter writer, IMicheline value, JsonSerializerOptions options)
        {
            var stack = new Stack<object>();
            stack.Push(value);

            int i;
            while (stack.Count > 0)
            {
                switch (stack.Pop())
                {
                    case MichelineArray array:
                        writer.WriteStartArray();
                        stack.Push(JsonTokenType.EndArray);
                        for (i = array.Count - 1; i >= 0; i--)
                            stack.Push(array[i]);
                        break;
                    case MichelinePrim prim:
                        writer.WriteStartObject();
                        writer.WriteString("prim", prim.Prim.ToString());
                        if (prim.Annots != null)
                        {
                            writer.WriteStartArray("annots");
                            foreach (var annot in prim.Annots)
                                writer.WriteStringValue(annot.ToString());
                            writer.WriteEndArray();
                        }
                        if (prim.Args != null)
                        {
                            writer.WriteStartArray("args");
                            stack.Push(JsonTokenType.EndObject);
                            stack.Push(JsonTokenType.EndArray);
                            for (i = prim.Args.Count - 1; i >= 0; i--)
                                stack.Push(prim.Args[i]);
                        }
                        else
                        {
                            writer.WriteEndObject();
                        }
                        break;
                    case MichelineBytes micheBytes:
                        writer.WriteStartObject();
                        writer.WriteString("bytes", Hex.Convert(micheBytes.Value));
                        writer.WriteEndObject();
                        break;
                    case MichelineString micheString:
                        writer.WriteStartObject();
                        writer.WriteString("string", micheString.Value);
                        writer.WriteEndObject();
                        break;
                    case MichelineInt micheInt:
                        writer.WriteStartObject();
                        writer.WriteString("int", micheInt.Value.ToString());
                        writer.WriteEndObject();
                        break;
                    case JsonTokenType token:
                        if (token == JsonTokenType.EndArray) writer.WriteEndArray();
                        else writer.WriteEndObject();
                        break;
                    default:
                        writer.WriteNullValue();
                        break;
                }
            }
        }
    }
}
