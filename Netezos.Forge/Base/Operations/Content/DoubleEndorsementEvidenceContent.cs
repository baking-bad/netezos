using Newtonsoft.Json;

namespace Netezos.Forge.Operations
{
    public class DoubleEndorsementEvidenceContent : OperationContent
    {
        [JsonProperty("kind")]
        public override string Kind => "double_endorsement_evidence";

        [JsonProperty("op1")]
        public DoubleEndorsementOperationContent Op1 { get; set; }

        [JsonProperty("op2")]
        public DoubleEndorsementOperationContent Op2 { get; set; }
    }
}
