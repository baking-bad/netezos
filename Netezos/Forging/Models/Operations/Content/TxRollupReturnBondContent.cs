using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class TxRollupReturnBondContent : ManagerOperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "tx_rollup_return_bond";

        [JsonPropertyName("rollup")]
        public string Rollup { get; set; } = null!;
    }
}
