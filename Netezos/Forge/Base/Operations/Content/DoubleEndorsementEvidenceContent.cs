using System.Text.Json.Serialization;

namespace Netezos.Forge.Operations
{
    public class DoubleEndorsementEvidenceContent : OperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "double_endorsement_evidence";

        [JsonPropertyName("op1")]
        public DoubleEndorsementOperationContent Op1 { get; set; }

        [JsonPropertyName("op2")]
        public DoubleEndorsementOperationContent Op2 { get; set; }
    }
}
