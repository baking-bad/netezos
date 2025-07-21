using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class UpdateCompanionKeyContent : ManagerOperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "update_companion_key";

        [JsonPropertyName("pk")]
        public required string PublicKey { get; set; }

        [JsonPropertyName("proof")]
        public string? Proof { get; set; }
    }
}
