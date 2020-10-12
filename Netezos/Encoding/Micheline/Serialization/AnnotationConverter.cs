using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Netezos.Encoding.Serialization
{
    public class AnnotationConverter : JsonConverter<IAnnotation>
    {
        public override IAnnotation Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var annot = reader.GetString();

            if (annot.Length < 1)
                throw new FormatException("Invalid annotation format");

            switch(annot[0])
            {
                case '%': return new FieldAnnotation(annot.Substring(1));
                case ':': return new TypeAnnotation(annot.Substring(1));
                case '@': return new VariableAnnotation(annot.Substring(1));
                default:
                    throw new FormatException("Invalid annotation prefix");
            }
        }

        public override void Write(Utf8JsonWriter writer, IAnnotation value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value?.ToString());
        }
    }
}
