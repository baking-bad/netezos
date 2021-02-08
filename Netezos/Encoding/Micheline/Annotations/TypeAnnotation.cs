namespace Netezos.Encoding
{
    public class TypeAnnotation : IAnnotation
    {
        public const char Prefix = ':';

        public AnnotationType Type => AnnotationType.Type;

        public string Value { get; set; }

        public TypeAnnotation(string value) => Value = value;

        public override string ToString() => $"{Prefix}{Value}";
    }
}
