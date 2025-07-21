using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class SrCementContent : ManagerOperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "smart_rollup_cement";

        [JsonPropertyName("rollup")]
        public required string Rollup { get; set; }
    }
}