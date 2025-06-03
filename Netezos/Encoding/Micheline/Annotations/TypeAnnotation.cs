namespace Netezos.Encoding
{
    public class TypeAnnotation(string value) : IAnnotation
    {
        public const char Prefix = ':';

        public AnnotationType Type => AnnotationType.Type;

        public string Value { get; set; } = value;

        public override string ToString() => $"{Prefix}{Value}";
    }
}
