using Netezos.Encoding.Serialization;

namespace Netezos.Encoding
{
    [InterfaceJsonConverter(typeof(AnnotationConverter))]
    public interface IAnnotation
    {
        AnnotationType Type { get; }
        string Value { get; }
    }

    public enum AnnotationType : byte
    {
        Field    = 0b_0100_0000,
        Type     = 0b_1000_0000,
        Variable = 0b_1100_0000
    }
}
