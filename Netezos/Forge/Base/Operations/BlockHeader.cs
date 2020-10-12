using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Netezos.Forge.Operations
{
    public class BlockHeader
    {
        [JsonPropertyName("level")]
        public int Level { get; }

        [JsonPropertyName("proto")]
        public int Proto { get; set; }
        
        [JsonPropertyName("predecessor")]
        public string Predecessor { get; set; }
        
        //TODO Think about it
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

        public BlockHeader(int level, int proto, string predecessor, DateTime timestamp, int validationPass, string operationsHash, List<string> fitness, string context, int priority, string proofOfWorkNonce, string signature)
        {
            Level = level;
            Proto = proto;
            Predecessor = predecessor;
            Timestamp = timestamp;
            ValidationPass = validationPass;
            OperationsHash = operationsHash;
            Fitness = fitness;
            Context = context;
            Priority = priority;
            ProofOfWorkNonce = proofOfWorkNonce;
            Signature = signature;
        }
    }
}