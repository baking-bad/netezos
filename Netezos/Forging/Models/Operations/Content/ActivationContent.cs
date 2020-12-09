using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class ActivationContent : OperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "activate_account";

        [JsonPropertyName("pkh")]
        public string Address { get; set; }

        [JsonPropertyName("secret")]
        public string Secret { get; set; }
    }
}
