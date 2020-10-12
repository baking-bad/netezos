namespace Netezos.Encoding
{
    public class TypeAnnotation : IAnnotation
    {
        public AnnotationType Type => AnnotationType.Type;

        public string Value { get; set; }

        public TypeAnnotation(string value) => Value = value;

        public override string ToString() => $":{Value}";
    }
}
