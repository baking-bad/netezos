namespace Netezos.Encoding
{
    public interface IAnnotation
    {
        AnnotationType Type { get; }
        string Value { get; }
    }
}
