namespace Netezos.Encoding
{
    public class FieldAnnotation : IAnnotation
    {
        public AnnotationType Type => AnnotationType.Field;

        public string Value { get; set; }

        public FieldAnnotation(string value) => Value = value;

        public override string ToString() => $"%{Value}";
    }
}
