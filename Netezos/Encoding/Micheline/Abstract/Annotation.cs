using System.Text.Json.Serialization;
using Netezos.Encoding.Serialization;

namespace Netezos.Encoding
{
    [JsonConverter(typeof(AnnotationConverter))]
    public interface IAnnotation
    {
        AnnotationType Type { get; }
        string Value { get; }
    }

    public enum AnnotationType : byte
    {
        Unsafe   = 0b_0000_0000,
        Field    = 0b_0100_0000,
        Type     = 0b_1000_0000,
        Variable = 0b_1100_0000
    }
}
