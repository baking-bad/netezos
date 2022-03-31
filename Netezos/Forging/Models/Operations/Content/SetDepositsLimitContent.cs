using System.Numerics;
using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class SetDepositsLimitContent : ManagerOperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "set_deposits_limit";

        [JsonPropertyName("limit")]
        [JsonConverter(typeof(BigIntegerNullableStringConverter))]
        public BigInteger? Limit { get; set; }
    }
}
