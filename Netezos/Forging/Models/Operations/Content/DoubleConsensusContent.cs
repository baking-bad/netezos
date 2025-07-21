using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class DoubleConsensusContent : OperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "double_consensus_operation_evidence";

        [JsonPropertyName("slot")]
        public ushort Slot { get; set; }

        [JsonPropertyName("op1")]
        public required InlineConsensusOperation Op1 { get; set; }

        [JsonPropertyName("op2")]
        public required InlineConsensusOperation Op2 { get; set; }
    }

    public class InlineConsensusOperation
    {
        [JsonPropertyName("branch")]
        public required string Branch { get; set; }

        [JsonPropertyName("operations")]
        public required ConsensusOperationContent Operations { get; set; }

        [JsonPropertyName("signature")]
        public required string Signature { get; set; }
    }
}
