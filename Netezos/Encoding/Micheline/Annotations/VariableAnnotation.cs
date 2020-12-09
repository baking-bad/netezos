namespace Netezos.Encoding
{
    public class VariableAnnotation : IAnnotation
    {
        public AnnotationType Type => AnnotationType.Variable;

        public string Value { get; set; }

        public VariableAnnotation(string value) => Value = value;

        public override string ToString() => $"@{Value}";
    }
}
