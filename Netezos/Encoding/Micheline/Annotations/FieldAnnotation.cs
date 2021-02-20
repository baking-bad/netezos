namespace Netezos.Encoding
{
    public class FieldAnnotation : IAnnotation
    {
        public const char Prefix = '%';

        public AnnotationType Type => AnnotationType.Field;

        public string Value { get; set; }

        public FieldAnnotation(string value) => Value = value;

        public override string ToString() => $"{Prefix}{Value}";
    }
}
