using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class SrExecuteContent : ManagerOperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "smart_rollup_execute_outbox_message";

        [JsonPropertyName("rollup")]
        public string Rollup { get; set; } = null!;

        [JsonPropertyName("cemented_commitment")]
        public string Commitment { get; set; } = null!;

        [JsonPropertyName("output_proof")]
        [JsonConverter(typeof(HexConverter))]
        public byte[] OutputProof { get; set; } = null!;
    }
}