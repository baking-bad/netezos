using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class SetDepositsLimitContent : ManagerOperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "set_deposits_limit";

        [JsonPropertyName("limit")]
        [JsonConverter(typeof(Int64NullableStringConverter))]
        public long? Limit { get; set; }
    }
}
