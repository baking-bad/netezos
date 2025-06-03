namespace Netezos.Encoding
{
    public class VariableAnnotation(string value) : IAnnotation
    {
        public const char Prefix = '@';

        public AnnotationType Type => AnnotationType.Variable;

        public string Value { get; set; } = value;

        public override string ToString() => $"{Prefix}{Value}";
    }
}
