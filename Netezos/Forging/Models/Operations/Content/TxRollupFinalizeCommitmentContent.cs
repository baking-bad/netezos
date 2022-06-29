using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class TxRollupFinalizeCommitmentContent : ManagerOperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "tx_rollup_finalize_commitment";

        [JsonPropertyName("rollup")]
        public string Rollup { get; set; }
    }
}
