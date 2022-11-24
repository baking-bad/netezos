using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class DrainDelegateContent : OperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "drain_delegate";

        [JsonPropertyName("consensus_key")]
        public string ConsensusKey { get; set; }

        [JsonPropertyName("delegate")]
        public string Delegate { get; set; }

        [JsonPropertyName("destination")]
        public string Destination { get; set; }
    }
}
