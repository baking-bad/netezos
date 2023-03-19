using System.Numerics;
using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class IncreasePaidStorageContent : ManagerOperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "increase_paid_storage";

        [JsonPropertyName("amount")]
        [JsonConverter(typeof(BigIntegerStringConverter))]
        public BigInteger Amount { get; set; }

        [JsonPropertyName("destination")]
        public string Destination { get; set; } = null!;
    }
}
