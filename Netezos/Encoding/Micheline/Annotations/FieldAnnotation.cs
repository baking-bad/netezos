namespace Netezos.Encoding
{
    public class FieldAnnotation(string value) : IAnnotation
    {
        public const char Prefix = '%';

        public AnnotationType Type => AnnotationType.Field;

        public string Value { get; set; } = value;

        public override string ToString() => $"{Prefix}{Value}";
    }
}
