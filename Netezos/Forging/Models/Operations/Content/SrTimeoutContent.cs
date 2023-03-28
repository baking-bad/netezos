using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class SrTmieoutContent : ManagerOperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "smart_rollup_timeout";

        [JsonPropertyName("rollup")]
        public string Rollup { get; set; } = null!;

        [JsonPropertyName("stakers")]
        public StakersPair Stakers { get; set; } = null!;
    }

    public class StakersPair
    {
        [JsonPropertyName("alice")]
        public string Alice { get; set; } = null!;

        [JsonPropertyName("bob")]
        public string Bob { get; set; } = null!;
    }
}