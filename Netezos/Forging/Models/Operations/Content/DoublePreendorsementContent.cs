using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class DoublePreendorsementContent : OperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "double_preendorsement_evidence";

        [JsonPropertyName("op1")]
        public InlinePreendorsement Op1 { get; set; } = null!;

        [JsonPropertyName("op2")]
        public InlinePreendorsement Op2 { get; set; } = null!;
    }

    public class InlinePreendorsement
    {
        [JsonPropertyName("branch")]
        public string Branch { get; set; } = null!;

        [JsonPropertyName("operations")]
        public PreendorsementContent Operations { get; set; } = null!;

        [JsonPropertyName("signature")]
        public string Signature { get; set; } = null!;
    }
}
