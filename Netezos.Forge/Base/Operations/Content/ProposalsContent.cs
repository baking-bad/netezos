using System.Collections.Generic;
using Newtonsoft.Json;

namespace Netezos.Forge.Operations
{
    public class ProposalsContent : OperationContent
    {
        [JsonProperty("kind")]
        public override string Kind => "proposals";
        
        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("period")]
        public int Period { get; set; }

        [JsonProperty("proposals")]
        public List<string> Proposals { get; set; }
    }
}
