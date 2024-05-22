using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class DoublePreattestationContent : OperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "double_preattestation_evidence";

        [JsonPropertyName("op1")]
        public InlinePreattestation Op1 { get; set; } = null!;

        [JsonPropertyName("op2")]
        public InlinePreattestation Op2 { get; set; } = null!;
    }

    public class InlinePreattestation
    {
        [JsonPropertyName("branch")]
        public string Branch { get; set; } = null!;

        [JsonPropertyName("operations")]
        public PreattestationContent Operations { get; set; } = null!;

        [JsonPropertyName("signature")]
        public string Signature { get; set; } = null!;
    }
}
