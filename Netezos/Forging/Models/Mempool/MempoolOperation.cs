using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class MempoolOperation
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