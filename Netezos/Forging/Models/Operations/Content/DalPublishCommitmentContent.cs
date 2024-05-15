using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class DalPublishCommitmentContent : ManagerOperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "dal_publish_commitment";

        [JsonPropertyName("slot_header")]
        public DalSlotHeader SlotHeader { get; set; } = null!;
    }

    public class DalSlotHeader
    {
        [JsonPropertyName("slot_index")]
        public byte SlotIndex { get; set; }

        [JsonPropertyName("commitment")]
        public string Commitment { get; set; } = null!;

        [JsonPropertyName("commitment_proof")]
        [JsonConverter(typeof(HexConverter))]
        public byte[] CommitmentProof { get; set; } = null!;
    }
}
