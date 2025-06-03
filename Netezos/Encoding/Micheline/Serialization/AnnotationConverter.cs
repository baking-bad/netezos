using System.Text.Json;
using System.Text.Json.Serialization;

namespace Netezos.Encoding.Serialization
{
    public class AnnotationConverter : JsonConverter<IAnnotation>
    {
        internal static IAnnotation ParseAnnotation(string annot)
        {
            if (annot.Length == 0)
                return new UnsafeAnnotation(annot);

            return annot[0] switch
            {
                '%' => new FieldAnnotation(annot[1..]),
                ':' => new TypeAnnotation(annot[1..]),
                '@' => new VariableAnnotation(annot[1..]),
                _ => new UnsafeAnnotation(annot)
            };
        }

        public override IAnnotation Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return ParseAnnotation(reader.GetString() ?? throw new FormatException("Cannot read from null"));
        }

        public override void Write(Utf8JsonWriter writer, IAnnotation value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value?.ToString());
        }
    }
}
