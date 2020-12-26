using Netezos.Encoding.Serialization;

namespace Netezos.Encoding
{
    [InterfaceJsonConverter(typeof(AnnotationConverter))]
    public interface IAnnotation
    {
        AnnotationType Type { get; }
        string Value { get; }
    }
}
