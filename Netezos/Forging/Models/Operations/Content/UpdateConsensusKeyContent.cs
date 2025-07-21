using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class UpdateConsensusKeyContent : ManagerOperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "update_consensus_key";

        [JsonPropertyName("pk")]
        public required string PublicKey { get; set; }

        [JsonPropertyName("proof")]
        public string? Proof { get; set; }
    }
}
