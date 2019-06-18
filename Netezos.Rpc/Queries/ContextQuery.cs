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
        public ConstantsQuery Constants => new ConstantsQuery(this, "constants/");

        /// <summary>
        /// Gets the query to all existing contracts (including non-empty default contracts)
        /// </summary>
        public ContractsQuery Contracts => new ContractsQuery(this, "contracts/");

        /// <summary>
        /// Gets the query to all registered delegates
        /// </summary>
        public DelegatesQuery Delegates => new DelegatesQuery(this, "delegates/");

        /// <summary>
        /// Gets the query to the info about the nonce of a previous block
        /// </summary>
        public NoncesQuery Nonces => new NoncesQuery(this, "nonces/");

        /// <summary>
        /// Gets the query to the raw context data
        /// </summary>
        public RawContextQuery Raw => new RawContextQuery(this, "raw/json/");
        
        /// <summary>
        /// Gets the query to the seed data
        /// </summary>
        public SeedQuery Seed => new SeedQuery(this, "seed/");

        internal ContextQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}
