using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class EndorsementContent : OperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "endorsement";

        [JsonPropertyName("slot")]
        public int Slot { get; set; }

        [JsonPropertyName("level")]
        public int Level { get; set; }

        [JsonPropertyName("round")]
        public int Round { get; set; }

        [JsonPropertyName("block_payload_hash")]
        public string PayloadHash { get; set; } = null!;
    }
}
