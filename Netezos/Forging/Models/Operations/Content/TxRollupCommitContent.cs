using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class TxRollupCommitContent : ManagerOperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "tx_rollup_commit";

        [JsonPropertyName("rollup")]
        public string Rollup { get; set; } = null!;

        [JsonPropertyName("commitment")]
        public TxRollupCommitment Commitment { get; set; } = null!;
    }

    public class TxRollupCommitment
    {
        [JsonPropertyName("level")]
        public int Level { get; set; }

        [JsonPropertyName("messages")]
        public List<string> Messages { get; set; } = null!;

        [JsonPropertyName("predecessor")]
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        public string? Predecessor { get; set; }

        [JsonPropertyName("inbox_merkle_root")]
        public string InboxMerkleRoot { get; set; } = null!;
    }
}
