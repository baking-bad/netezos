using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class AttestationContent : ConsensusOperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "attestation";

        [JsonPropertyName("slot")]
        public ushort Slot { get; set; }

        [JsonPropertyName("level")]
        public int Level { get; set; }

        [JsonPropertyName("round")]
        public int Round { get; set; }

        [JsonPropertyName("block_payload_hash")]
        public required string PayloadHash { get; set; }
    }
}
