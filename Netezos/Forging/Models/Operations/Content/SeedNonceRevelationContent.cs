using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class SeedNonceRevelationContent : OperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "seed_nonce_revelation";

        [JsonPropertyName("level")]
        public int Level { get; set; }

        [JsonPropertyName("nonce")]
        public string Nonce { get; set; }
    }
}
