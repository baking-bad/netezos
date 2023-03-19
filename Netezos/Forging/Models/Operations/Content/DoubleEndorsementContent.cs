using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class DoubleEndorsementContent : OperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "double_endorsement_evidence";

        [JsonPropertyName("op1")]
        public InlineEndorsement Op1 { get; set; } = null!;

        [JsonPropertyName("op2")]
        public InlineEndorsement Op2 { get; set; } = null!;
    }

    public class InlineEndorsement
    {
        [JsonPropertyName("branch")]
        public string Branch { get; set; } = null!;

        [JsonPropertyName("operations")]
        public EndorsementContent Operations { get; set; } = null!;

        [JsonPropertyName("signature")]
        public string Signature { get; set; } = null!;
    }
}
