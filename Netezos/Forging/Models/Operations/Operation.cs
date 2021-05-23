using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class Operation
    {
        [JsonPropertyName("hash")]
        public string Hash { get; set; }

        [JsonPropertyName("branch")]
        public string Branch { get; set; }

        [JsonPropertyName("contents")]
        public List<OperationContent> Contents { get; set; }

        [JsonPropertyName("signature")]
        public string Signature { get; set; }
    }
}
