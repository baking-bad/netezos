using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class BlockHeaderContent
    {
        [JsonPropertyName("protocol_data")]
        public ActivationProtocolDataContent ProtocolData { get; set; }
        [JsonPropertyName("operations")]
        public List<HeaderOperationContent> Operations { get; set; }
    }
    
    public class HeaderOperationContent
    {
        [JsonPropertyName("protocol")]
        public string Protocol { get; set; }
        [JsonPropertyName("branch")]
        public string Branch { get; set; }
        [JsonPropertyName("contents")]
        public List<OperationContent> Contents { get; set; }
        [JsonPropertyName("signature")]
        public string Signature { get; set; }
    }
    
}