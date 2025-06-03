namespace Netezos.Encoding
{
    public class UnsafeAnnotation(string value) : IAnnotation
    {
        public AnnotationType Type => AnnotationType.Unsafe;

        public string Value { get; set; } = value;

        public override string ToString() => Value;
    }
}
