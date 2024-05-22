using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class DoubleAttestationContent : OperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "double_attestation_evidence";

        [JsonPropertyName("op1")]
        public InlineAttestation Op1 { get; set; } = null!;

        [JsonPropertyName("op2")]
        public InlineAttestation Op2 { get; set; } = null!;
    }

    public class InlineAttestation
    {
        [JsonPropertyName("branch")]
        public string Branch { get; set; } = null!;

        [JsonPropertyName("operations")]
        public AttestationContent Operations { get; set; } = null!;

        [JsonPropertyName("signature")]
        public string Signature { get; set; } = null!;
    }
}
