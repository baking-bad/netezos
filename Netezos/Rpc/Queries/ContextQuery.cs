using Netezos.Rpc.Queries.Post;

namespace Netezos.Rpc.Queries
{
    /// <summary>
    /// Rpc query to access context data
    /// </summary>
    public class ContextQuery : RpcQuery
    {
        /// <summary>
        /// Gets the query to the protocol's constants
        /// </summary>
        public RpcSimpleArray<RpcSimpleDictionary<string, BigMapsQuery>> BigMaps => new(this, "big_maps/");

        /// <summary>
        /// Gets the query to the protocol's constants
        /// </summary>
        public ConstantsQuery Constants => new(this, "constants/");

        /// <summary>
        /// Gets the query to all existing contracts (including non-empty default contracts)
        /// </summary>
        public ContractsQuery Contracts => new(this, "contracts/");

        /// <summary>
        /// Gets the query to all registered delegates
        /// </summary>
        public DelegatesQuery Delegates => new(this, "delegates/");

        /// <summary>
        /// Gets the query to the info about the nonce of a previous block
        /// </summary>
        public NoncesQuery Nonces => new(this, "nonces/");

        /// <summary>
        /// Gets the query to the raw context data
        /// </summary>
        public RawContextQuery Raw => new(this, "raw/json/");
        
        /// <summary>
        /// Gets the query to the seed data
        /// </summary>
        public SeedQuery Seed => new(this, "seed");

        internal ContextQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}
