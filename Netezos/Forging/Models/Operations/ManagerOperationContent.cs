using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public abstract class ManagerOperationContent : OperationContent
    {
        [JsonPropertyName("source")]
        public string Source { get; set; }

        [JsonPropertyName("fee")]
        public long Fee { get; set; }

        [JsonPropertyName("counter")]
        public int Counter { get; set; }

        [JsonPropertyName("gas_limit")]
        public int GasLimit { get; set; }

        [JsonPropertyName("storage_limit")]
        public int StorageLimit { get; set; }
    }
}
