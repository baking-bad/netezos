namespace Netezos.Sandbox.Models
{
    public class SandboxConstants
    {
        public int GasReserve { get; set; }
        public int BurnReserve { get; set; }
        public int? Counter { get; set; }
        public int? Ttl { get; set; }
        public int? Fee { get; set; }
        public int? GasLimit { get; set; }
        public int? StorageLimit { get; set; }
        public int? BranchOffset { get; set; }
    }
}