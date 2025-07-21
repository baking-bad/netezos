using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class DalPublishCommitmentContent : ManagerOperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "dal_publish_commitment";

        [JsonPropertyName("slot_header")]
        public required DalSlotHeader SlotHeader { get; set; }
    }

    public class DalSlotHeader
    {
        [JsonPropertyName("slot_index")]
        public byte SlotIndex { get; set; }

        [JsonPropertyName("commitment")]
        public required string Commitment { get; set; }

        [JsonPropertyName("commitment_proof")]
        [JsonConverter(typeof(HexConverter))]
        public required byte[] CommitmentProof { get; set; }
    }
}
