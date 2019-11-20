using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Netezos.Forge.Operations
{
    public class BlockHeader
    {
        [JsonProperty("level")]
        public int Level { get; }

        [JsonProperty("proto")]
        public int Proto { get; set; }
        
        [JsonProperty("predecessor")]
        public string Predecessor { get; set; }
        
        //TODO Think about it
        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }
        
        [JsonProperty("validation_pass")]
        public int ValidationPass { get; set; }
        
        [JsonProperty("operations_hash")]
        public string OperationsHash { get; set; }
        
        [JsonProperty("fitness")]
        public List<string> Fitness { get; set; }
        
        [JsonProperty("context")]
        public string Context { get; set; }
        
        [JsonProperty("priority")]
        public int Priority { get; set; }
        
        [JsonProperty("proof_of_work_nonce")]
        public string ProofOfWorkNonce { get; set; }
        
        [JsonProperty("signature")]
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