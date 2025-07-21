using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class DalEntrapmentEvidenceContent : OperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "dal_entrapment_evidence";

        [JsonPropertyName("attestation")]
        public InlineAttestation Attestation { get; set; } = null!;

        [JsonPropertyName("slot_index")]
        public byte SlotIndex { get; set; }
        
        [JsonPropertyName("shard_with_proof")]
        public required ShardWithProof ShardWithProof { get; set; }
    }
    
    public class ShardWithProof
    {
        [JsonPropertyName("shard")]
        [JsonConverter(typeof(ShardDataConverter))]
        public required ShardData Shard { get; set; }

        [JsonPropertyName("proof")]
        public required string Proof { get; set; }
    }
    
    public class ShardData
    {
        public int Id { get; set; }
        public required List<string> Hashes { get; set; }
    }
}
