using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class SrPublishContent : ManagerOperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "smart_rollup_publish";

        [JsonPropertyName("rollup")]
        public string Rollup { get; set; } = null!;

        [JsonPropertyName("commitment")]
        public Commitment Commitment { get; set; } = null!;
    }

    public class Commitment
    {
        [JsonPropertyName("predecessor")]
        public string Predecessor { get; set; } = null!;

        [JsonPropertyName("compressed_state")]
        public string State { get; set; } = null!;

        [JsonPropertyName("inbox_level")]
        public int InboxLevel { get; set; }

        [JsonPropertyName("number_of_ticks")]
        [JsonConverter(typeof(Int64StringConverter))]
        public long Ticks { get; set; }
    }
}