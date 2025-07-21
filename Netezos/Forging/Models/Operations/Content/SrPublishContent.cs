using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class SrPublishContent : ManagerOperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "smart_rollup_publish";

        [JsonPropertyName("rollup")]
        public required string Rollup { get; set; }

        [JsonPropertyName("commitment")]
        public required Commitment Commitment { get; set; }
    }

    public class Commitment
    {
        [JsonPropertyName("predecessor")]
        public required string Predecessor { get; set; }

        [JsonPropertyName("compressed_state")]
        public required string State { get; set; }

        [JsonPropertyName("inbox_level")]
        public int InboxLevel { get; set; }

        [JsonPropertyName("number_of_ticks")]
        [JsonConverter(typeof(Int64StringConverter))]
        public long Ticks { get; set; }
    }
}