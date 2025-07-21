using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class ActivationContent : OperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "activate_account";

        [JsonPropertyName("pkh")]
        public required string Address { get; set; }

        [JsonPropertyName("secret")]
        public required string Secret { get; set; }
    }
}
