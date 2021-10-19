
using System.Text.Json.Serialization;

namespace Netezos.Sandbox.Models
{
    public class SandboxConstants
    {
        [JsonPropertyName("gas_reserve")]
        public int GasReserve { get; set; }
        [JsonPropertyName("burn_reserve")]
        public int BurnReserve { get; set; }
        [JsonPropertyName("counter")]
        public int? Counter { get; set; }
        [JsonPropertyName("ttl")]
        public int? Ttl { get; set; }
        [JsonPropertyName("fee")]
        public int? Fee { get; set; }
        [JsonPropertyName("gas_limit")]
        public int? GasLimit { get; set; }
        [JsonPropertyName("storage_limit")]
        public int? StorageLimit { get; set; }
        [JsonPropertyName("branch_offset")]
        public int? BranchOffset { get; set; }
    }
}