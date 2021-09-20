using System.Collections.Generic;
using Netezos.Forging.Models;
using Netezos.Keys;

namespace Netezos.Sandbox.Models
{
    public class BlockParameters
    {
        public Key Key { get; set; }
        public string Branch { get; set; }
        public string ChainId { get; set; }
        public string Protocol { get; set; }
        public int Counter { get; set; }
        public string Signature { get; set; }
        public List<OperationContent> Operations { get; set; }
        public bool IsSandbox { get; set; }
        public int? BranchOffset { get; set; }
        public int? Ttl { get; set; }
        public SandboxConstants Constants { get; set; }
    }
}