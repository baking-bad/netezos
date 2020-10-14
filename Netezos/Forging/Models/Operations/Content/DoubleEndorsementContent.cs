using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class DoubleEndorsementContent : OperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "double_endorsement_evidence";

        [JsonPropertyName("op1")]
        public InlinedEndorsement Op1 { get; set; }

        [JsonPropertyName("op2")]
        public InlinedEndorsement Op2 { get; set; }
    }

    public class InlinedEndorsement
    {
        [JsonPropertyName("branch")]
        public string Branch { get; set; }

        [JsonPropertyName("operations")]
        public EndorsementContent Operations { get; set; }

        [JsonPropertyName("signature")]
        public string Signature { get; set; }
    }
}
