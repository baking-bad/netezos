using Netezos.Rpc.Queries.Post;

namespace Netezos.Rpc.Queries
{
    /// <summary>
    /// Rpc query to access context data
    /// </summary>
    public class ContextQuery : RpcQuery
    {
        /// <summary>
        /// Gets the query to get the cycle at which the launch of the Adaptive Issuance feature is set to happen. A result of None means that the feature is not yet set to launch.
        /// </summary>
        public RpcObject AdaptiveIssuanceLaunch => new(this, "adaptive_issuance_launch_cycle/");

        /// <summary>
        /// Gets the query to big maps.
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
        /// Gets the query to the issuance data
        /// </summary>
        public IssuanceQuery Issuance => new(this, "issuance/");

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
        
        /// <summary>
        /// Gets the query to the smart rollup data
        /// </summary>
        public SmartRollupsQuery SmartRollups => new(this, "smart_rollups/");
        
        /// <summary>
        /// Returns the total stake (in mutez) frozen on the chain.
        /// </summary>
        public RpcObject TotalFrozenStake => new(this, "total_frozen_stake/");
        
        /// <summary>
        /// Returns the total supply (in mutez) available on the chain.
        /// </summary>
        public RpcObject TotalSupply => new(this, "total_supply/");

        internal ContextQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}
