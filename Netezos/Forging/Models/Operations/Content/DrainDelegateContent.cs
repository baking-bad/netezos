using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class DrainDelegateContent : OperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "drain_delegate";

        [JsonPropertyName("consensus_key")]
        public string ConsensusKey { get; set; } = null!;

        [JsonPropertyName("delegate")]
        public string Delegate { get; set; } = null!;

        [JsonPropertyName("destination")]
        public string Destination { get; set; } = null!;
    }
}
