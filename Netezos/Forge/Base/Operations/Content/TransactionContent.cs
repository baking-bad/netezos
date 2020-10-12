using System.Text.Json.Serialization;

namespace Netezos.Forge.Operations
{
    public class TransactionContent : ManagerOperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "transaction";

        [JsonPropertyName("amount")]
        public long Amount { get; set; }

        [JsonPropertyName("destination")]
        public string Destination { get; set; }

        [JsonPropertyName("parameters")]
        public Parameters Parameters { get; set; }
    }
}