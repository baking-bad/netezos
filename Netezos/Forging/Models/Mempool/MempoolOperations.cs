using System.Collections.Generic;
using System.Text.Json.Serialization;
using Netezos.Utils.Json;

namespace Netezos.Forging.Models
{
    /// <summary>
    /// The node maintains a memory pool (“mempool”) to keep track of not-invalid-for-sure operations.
    /// The mempool keeps track of different kinds of operations
    /// </summary>
    public class MempoolOperations
    {
        [JsonPropertyName("applied")]
        public List<Operation> Applied { get; set; }
        [JsonPropertyName("refused")]
        public List<Operation> Refused { get; set; }
        /// <summary>
        /// Operations which could be valid in a different branch
        /// </summary>
        [JsonPropertyName("branch_refused")]
        public List<Operation> BranchRefused { get; set; }
        /// <summary>
        /// Operation which has come too soon (ie there's a gap in the account counter)
        /// </summary>
        [JsonPropertyName("branch_delayed")]
        public List<Operation> BranchDelayed { get; set; }
        /// <summary>
        /// Invalid operations
        /// </summary>
        [JsonPropertyName("unprocessed")]
        public List<Operation> Unprocessed { get; set; }
    }
}