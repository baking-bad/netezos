namespace Netezos.Encoding
{
    public class UnsafeAnnotation : IAnnotation
    {
        public AnnotationType Type => AnnotationType.Unsafe;

        public string Value { get; set; }

        public UnsafeAnnotation(string value) => Value = value;

        public override string ToString() => Value;
    }
}
