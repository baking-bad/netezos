using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class RevealContent : ManagerOperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "reveal";

        [JsonPropertyName("public_key")]
        public required string PublicKey { get; set; }

        [JsonPropertyName("proof")]
        public string? Proof { get; set; }
    }
}
