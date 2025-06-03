using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class TxRollupOriginationContent : ManagerOperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "tx_rollup_origination";

        [JsonPropertyName("tx_rollup_origination")]
        public object Parameters { get; } = new();
    }
}
