using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class ProposalsContent : OperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "proposals";
        
        [JsonPropertyName("source")]
        public string Source { get; set; }

        [JsonPropertyName("period")]
        public int Period { get; set; }

        [JsonPropertyName("proposals")]
        public List<string> Proposals { get; set; }
    }
}
