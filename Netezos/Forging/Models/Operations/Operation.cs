using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class Operation
    {
        [JsonPropertyName("hash")]
        public string Hash { get; set; } = null!;

        [JsonPropertyName("branch")]
        public string Branch { get; set; } = null!;

        [JsonPropertyName("contents")]
        public List<OperationContent> Contents { get; set; } = null!;

        [JsonPropertyName("signature")]
        public string Signature { get; set; } = null!;
    }
}
