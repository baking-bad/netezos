using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class PreattestationsAggregateContent : ConsensusOperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "preattestations_aggregate";

        [JsonPropertyName("consensus_content")]
        public required ConsensusContent ConsensusContent { get; set; }

        [JsonPropertyName("committee")]
        public required List<ushort> Committee { get; set; }
    }

    public class ConsensusContent
    {
        [JsonPropertyName("level")]
        public int Level { get; set; }

        [JsonPropertyName("round")]
        public int Round { get; set; }

        [JsonPropertyName("block_payload_hash")]
        public required string PayloadHash { get; set; }
    }
}
