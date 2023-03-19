namespace Netezos.Rpc.Queries
{
    /// <summary>
    /// Rpc query to access contract data
    /// </summary>
    public class RawContractQuery : DeepRpcObject
    {
        /// <summary>
        /// Gets the query to the balance of a contract
        /// </summary>
        public RpcObject Balance => new(this, "balance/");

        /// <summary>
        /// Gets the query to the value associated with a key in the big map storage of the contract
        /// </summary>
        public RpcObject BigMap => new(this, "big_map/");

        /// <summary>
        /// Gets the query to contract's code
        /// </summary>
        public RpcObject Code => new(this, "code/");

        /// <summary>
        /// Gets the query to contract's change
        /// </summary>
        [Obsolete("This RPC query was removed. Use it on early protocols only.")]
        public RpcObject Change => new(this, "change/");

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
        /// 
        /// </summary>
        public RpcObject DelegateDesactivation => new(this, "delegate_desactivation/");

        /// <summary>
        /// 
        /// </summary>
        public RpcObject InactiveDelegate => new(this, "inactive_delegate/");

        /// <summary>
        /// Gets the query to all contracts delegated to this contract
        /// </summary>
        public DeepRpcDictionary<string, RpcObject> Delegated => new(this, "delegated/");

        /// <summary>
        /// Gets the query to the frozen balances
        /// </summary>
        [Obsolete("This RPC query was removed. Use it on early protocols only.")]
        public DeepRpcDictionary<int, FrozenBalanceQuery> FrozenBalance => new(this, "frozen_balance/");

        /// <summary>
        /// Gets the query to the frozen deposits
        /// </summary>
        public RpcObject FrozenDeposits => new(this, "frozen_deposits/");
        
        /// <summary>
        /// Gets the query to the manager of a contract
        /// </summary>
        public RpcObject Manager => new(this, "manager/");

        /// <summary>
        /// Gets the query to the paid bytes
        /// </summary>
        public RpcObject PaidBytes => new(this, "paid_bytes/");

        /// <summary>
        /// Gets the query to the roll list
        /// </summary>
        [Obsolete("This RPC query was removed. Use it on early protocols only.")]
        public RpcObject RollList => new(this, "roll_list/");

        /// <summary>
        /// Gets the query to the flag, indicating if the contract tokens can be spent by the manager
        /// </summary>
        [Obsolete("This RPC query was removed. Use it on early protocols only.")]
        public RpcObject Spendable => new(this, "spendable/");

        /// <summary>
        /// Gets the query to the data of the contract's storage
        /// </summary>
        public RpcObject Storage => new(this, "storage/");

        /// <summary>
        /// Gets the query to the used bytes
        /// </summary>
        public RpcObject UsedBytes => new(this, "used_bytes/");

        internal RawContractQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}
