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
        public ShardWithProof ShardWithProof { get; set; } = null!;
    }
    
    public class ShardWithProof
    {
        [JsonPropertyName("shard")]
        [JsonConverter(typeof(ShardDataConverter))]
        public ShardData Shard { get; set; } = null!;

        [JsonPropertyName("proof")]
        public string Proof { get; set; } = null!;
    }
    
    public class ShardData
    {
        public int Id { get; set; }
        public List<string> Hashes { get; set; } = null!;
    }
}
