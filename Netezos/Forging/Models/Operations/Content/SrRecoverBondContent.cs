using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class SrRecoverBondContent : ManagerOperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "smart_rollup_recover_bond";

        [JsonPropertyName("rollup")]
        public required string Rollup { get; set; }

        [JsonPropertyName("staker")]
        public required string Staker { get; set; }
    }
}