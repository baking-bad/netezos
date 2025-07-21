using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class Operation
    {
        [JsonPropertyName("hash")]
        public required string Hash { get; set; }

        [JsonPropertyName("branch")]
        public required string Branch { get; set; }

        [JsonPropertyName("contents")]
        public required List<OperationContent> Contents { get; set; }

        [JsonPropertyName("signature")]
        public required string Signature { get; set; }
    }
}
