using Netezos.Rpc.Queries.Post;

namespace Netezos.Rpc.Queries
{
    /// <summary>
    /// Rpc query to access contract data
    /// </summary>
    public class ContractQuery : RpcObject
    {
        /// <summary>
        /// Gets the query to the balance of a contract
        /// </summary>
        public RpcObject Balance => new(this, "balance/");

        /// <summary>
        /// Gets the query to the value associated with a key in the big map storage of the contract
        /// </summary>
        [Obsolete("This RPC query is deprecated. Use it on early protocols only.")]
        public BigMapQuery BigMap => new(this, "big_map_get/");

        /// <summary>
        /// Gets the query to the counter of a contract, if any
        /// </summary>
        public RpcObject Counter => new(this, "counter/");

        /// <summary>
        /// Gets the query to the flag, indicating if the contract delegate can be changed
        /// </summary>
        [Obsolete("This RPC query was removed. Use it on early protocols only.")]
        public RpcObject Delegatable => new(this, "delegatable/");

        /// <summary>
        /// Gets the query to the delegate of a contract, if any
        /// </summary>
        public RpcObject Delegate => new(this, "delegate/");

        /// <summary>
        /// Return the list of entrypoints of the contract
        /// </summary>
        public EntrypointsQuery Entrypoints => new(this, "entrypoints/");

        /// <summary>
        /// Gets the query to the manager of a contract
        /// </summary>
        [Obsolete("This RPC query was removed. Use it on early protocols only.")]
        public RpcObject Manager => new(this, "manager/");

        /// <summary>
        /// Gets the query to the manager of a contract and his key
        /// </summary>
        public RpcObject ManagerKey => new(this, "manager_key/");

        /// <summary>
        /// Gets the query to the code and data of the contract
        /// </summary>
        public ScriptQuery Script => new(this, "script/");

        /// <summary>
        /// Gets the query to the flag, indicating if the contract tokens can be spent by the manager
        /// </summary>
        [Obsolete("This RPC query was removed. Use it on early protocols only.")]
        public RpcObject Spendable => new(this, "spendable/");

        /// <summary>
        /// Gets the query to the data of the contract's storage
        /// </summary>
        public StorageQuery Storage => new(this, "storage/");

        internal ContractQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}
