using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class SrTimeoutContent : ManagerOperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "smart_rollup_timeout";

        [JsonPropertyName("rollup")]
        public required string Rollup { get; set; }

        [JsonPropertyName("stakers")]
        public required StakersPair Stakers { get; set; }
    }

    public class StakersPair
    {
        [JsonPropertyName("alice")]
        public required string Alice { get; set; }

        [JsonPropertyName("bob")]
        public required string Bob { get; set; }
    }
}