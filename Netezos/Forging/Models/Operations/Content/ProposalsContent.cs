using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class ProposalsContent : OperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "proposals";
        
        [JsonPropertyName("source")]
        public required string Source { get; set; }

        [JsonPropertyName("period")]
        public int Period { get; set; }

        [JsonPropertyName("proposals")]
        public required List<string> Proposals { get; set; }
    }
}
