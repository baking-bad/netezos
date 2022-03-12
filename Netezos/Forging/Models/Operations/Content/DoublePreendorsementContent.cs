using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class DoublePreendorsementContent : OperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "double_preendorsement_evidence";

        [JsonPropertyName("op1")]
        public InlinedPreendorsement Op1 { get; set; }

        [JsonPropertyName("op2")]
        public InlinedPreendorsement Op2 { get; set; }
    }

    public class InlinedPreendorsement
    {
        [JsonPropertyName("branch")]
        public string Branch { get; set; }

        [JsonPropertyName("operations")]
        public PreendorsementContent Operations { get; set; }

        [JsonPropertyName("signature")]
        public string Signature { get; set; }
    }
}
