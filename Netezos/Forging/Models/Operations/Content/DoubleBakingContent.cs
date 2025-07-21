using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class DoubleBakingContent : OperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "double_baking_evidence";

        [JsonPropertyName("bh1")]
        public required BlockHeader BlockHeader1 { get; set; }

        [JsonPropertyName("bh2")]
        public required BlockHeader BlockHeader2 { get; set; }
    }

    public class BlockHeader
    {
        [JsonPropertyName("level")]
        public int Level { get; set; }

        [JsonPropertyName("proto")]
        public int Proto { get; set; }

        [JsonPropertyName("predecessor")]
        public required string Predecessor { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonPropertyName("validation_pass")]
        public int ValidationPass { get; set; }

        [JsonPropertyName("operations_hash")]
        public required string OperationsHash { get; set; }

        [JsonPropertyName("fitness")]
        public required List<string> Fitness { get; set; }

        [JsonPropertyName("context")]
        public required string Context { get; set; }

        [JsonPropertyName("payload_hash")]
        public required string PayloadHash { get; set; }

        [JsonPropertyName("payload_round")]
        public int PayloadRound { get; set; }

        [JsonPropertyName("proof_of_work_nonce")]
        public required string ProofOfWorkNonce { get; set; }

        [JsonPropertyName("seed_nonce_hash")]
        public string? SeedNonceHash { get; set; }

        [JsonPropertyName("liquidity_baking_toggle_vote")]
        public LBToggle LiquidityBakingToggleVote { get; set; }

        [JsonPropertyName("signature")]
        public required string Signature { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum LBToggle
    {
        on = 0,
        off = 1,
        pass = 2
    }
}
