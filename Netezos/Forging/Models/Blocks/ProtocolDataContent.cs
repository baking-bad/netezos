using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public abstract class ProtocolDataContent
    {
        [JsonPropertyName("protocol")]
        public string ProtocolHash { get; set; }
        [JsonPropertyName("priority")]
        public int Priority { get; set; }
        [JsonPropertyName("proof_of_work_nonce")]
        public string ProofOfWorkNonce { get; set; }
        [JsonPropertyName("seed_nonce_hash")]
        public string SeedNonceHash { get; set; }
        [JsonPropertyName("signature")]
        public string Signature { get; set; }
    }
    
}