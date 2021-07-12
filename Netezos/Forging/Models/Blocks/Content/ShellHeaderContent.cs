using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class ShellHeaderContent
    {
        [JsonPropertyName("protocol")]
        public string Protocol { get; set; }
        [JsonPropertyName("chain_id")]
        public string ChainId { get; set; }
        [JsonPropertyName("level")]
        public int Level { get; set; }
        [JsonPropertyName("proto")]
        public int Proto { get; set; }
        [JsonPropertyName("predecessor")]
        public string Predecessor { get; set; }
        [JsonPropertyName("hash")]
        public string Hash { get; set; }
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
        [JsonPropertyName("priority")]
        public int Priority { get; set; }
        [JsonPropertyName("proof_of_work_nonce")]
        public string ProofOfWorkNonce { get; set; }
        [JsonPropertyName("signature")]
        public string Signature { get; set; }
    }
    
    public class ShellHeaderWithOperations
    {
        [JsonPropertyName("shell_header")]
        public ShellHeaderContent ShellHeader { get; set; }
        [JsonPropertyName("operations")]
        public List<HeaderOperationContent> Operations { get; set; }
    }
}