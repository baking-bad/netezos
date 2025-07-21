using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class SrExecuteContent : ManagerOperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "smart_rollup_execute_outbox_message";

        [JsonPropertyName("rollup")]
        public required string Rollup { get; set; }

        [JsonPropertyName("cemented_commitment")]
        public required string Commitment { get; set; }

        [JsonPropertyName("output_proof")]
        [JsonConverter(typeof(HexConverter))]
        public required byte[] OutputProof { get; set; }
    }
}