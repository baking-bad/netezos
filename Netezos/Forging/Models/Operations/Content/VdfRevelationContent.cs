using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class VdfRevelationContent : OperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "vdf_revelation";

        [JsonPropertyName("solution")]
        public required List<string> Solution { get; set; }
    }
}
