using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class DoubleBakingContent : OperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "double_baking_evidence";

        [JsonPropertyName("bh1")]
        public BlockHeader BlockHeader1 { get; set; }

        [JsonPropertyName("bh2")]
        public BlockHeader BlockHeader2 { get; set; }
    }

    public class BlockHeader
    {
        [JsonPropertyName("level")]
        public int Level { get; set; }

        [JsonPropertyName("proto")]
        public int Proto { get; set; }

        [JsonPropertyName("predecessor")]
        public string Predecessor { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonPropertyName("validation_pass")]
        public int ValidationPass { get; set; }

        [JsonPropertyName("operations_hash")]
        public string OperationsHash { get; set; }

        [JsonPropertyName("fitness")]
        public List<string> Fitness { get; set; }

        [JsonPropertyName("context")]
        public string Context { get; set; }

        [JsonPropertyName("payload_hash")]
        public string PayloadHash { get; set; }

        [JsonPropertyName("payload_round")]
        public int PayloadRound { get; set; }

        [JsonPropertyName("proof_of_work_nonce")]
        public string ProofOfWorkNonce { get; set; }

        [JsonPropertyName("seed_nonce_hash")]
        public string SeedNonceHash { get; set; }

        [JsonPropertyName("liquidity_baking_toggle_vote")]
        public LBToggle LiquidityBakingToggleVote { get; set; }

        [JsonPropertyName("signature")]
        public string Signature { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum LBToggle
    {
        On = 0,
        Off = 1,
        Pass = 2
    }
}
