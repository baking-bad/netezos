using System;

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
        public RpcObject Balance => new RpcObject(this, "balance/");

        /// <summary>
        /// Gets the query to the value associated with a key in the big map storage of the contract
        /// </summary>
        public RpcObject BigMap => new RpcObject(this, "big_map/");

        /// <summary>
        /// Gets the query to contract's code
        /// </summary>
        public RpcObject Code => new RpcObject(this, "code/");

        /// <summary>
        /// Gets the query to contract's change
        /// </summary>
        public RpcObject Change => new RpcObject(this, "change/");

        /// <summary>
        /// Gets the query to the counter of a contract, if any
        /// </summary>
        public RpcObject Counter => new RpcObject(this, "counter/");

        /// <summary>
        /// Gets the query to the flag, indicating if the contract delegate can be changed
        /// </summary>
        [Obsolete("This RPC query was removed. Use it on early protocols only.")]
        public RpcObject Delegatable => new RpcObject(this, "delegatable/");

        /// <summary>
        /// Gets the query to the delegate of a contract, if any
        /// </summary>
        public RpcObject Delegate => new RpcObject(this, "delegate/");

        /// <summary>
        /// 
        /// </summary>
        public RpcObject DelegateDesactivation
            => new RpcObject(this, "delegate_desactivation/");

        /// <summary>
        /// 
        /// </summary>
        public RpcObject InactiveDelegate
            => new RpcObject(this, "inactive_delegate/");

        /// <summary>
        /// Gets the query to all contracts delegated to this contract
        /// </summary>
        public DeepRpcDictionary<string, RpcObject>
            Delegated => new DeepRpcDictionary<string, RpcObject>(this, "delegated/");

        /// <summary>
        /// Gets the query to the frozen balances
        /// </summary>
        public DeepRpcDictionary<int, FrozenBalanceQuery>
            FrozenBalance => new DeepRpcDictionary<int, FrozenBalanceQuery>(this, "frozen_balance/");

        /// <summary>
        /// Gets the query to the manager of a contract
        /// </summary>
        public RpcObject Manager => new RpcObject(this, "manager/");

        /// <summary>
        /// Gets the query to the paid bytes
        /// </summary>
        public RpcObject PaidBytes => new RpcObject(this, "paid_bytes/");

        /// <summary>
        /// Gets the query to the roll list
        /// </summary>
        public RpcObject RollList => new RpcObject(this, "roll_list/");

        /// <summary>
        /// Gets the query to the flag, indocating if the contract tokens can be spent by the manager
        /// </summary>
        [Obsolete("This RPC query was removed. Use it on early protocols only.")]
        public RpcObject Spendable => new RpcObject(this, "spendable/");

        /// <summary>
        /// Gets the query to the data of the contract's storage
        /// </summary>
        public RpcObject Storage => new RpcObject(this, "storage/");

        /// <summary>
        /// Gets the query to the used bytes
        /// </summary>
        public RpcObject UsedBytes => new RpcObject(this, "used_bytes/");

        internal RawContractQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}
