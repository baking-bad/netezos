namespace Netezos.Rpc.Queries
{
    /// <summary>
    /// Rpc query to access contract data
    /// </summary>
    public class DelegateQuery : RpcObject
    {
        /// <summary>
        /// Gets the query to the full balance of a given delegate, including the frozen balances
        /// </summary>
        public RpcObject Balance => new RpcObject(this, "balance/");

        /// <summary>
        /// Gets the query to the flag, indicating whether the delegate is currently tagged as deactivated or not
        /// </summary>
        public RpcObject Deactivated => new RpcObject(this, "deactivated/");

        /// <summary>
        /// Gets the query to the balances of all the contracts that delegate to a given delegate. This excludes the delegate's own balance and its frozen balances
        /// </summary>
        public RpcObject DelegatedBalance => new RpcObject(this, "delegated_balance/");

        /// <summary>
        /// Gets the query to the list of contracts that delegate to a given delegate
        /// </summary>
        public RpcObject DelegatedContracts => new RpcObject(this, "delegated_contracts/");

        /// <summary>
        /// Gets the query to the total frozen balances of a given delegate, this includes the frozen deposits, rewards and fees
        /// </summary>
        public RpcObject FrozenBalance => new RpcObject(this, "frozen_balance/");

        /// <summary>
        /// Gets the query to the frozen balances of a given delegate, indexed by the cycle by which it will be unfrozen
        /// </summary>
        public RpcObject FrozenBalanceByCycle => new RpcObject(this, "frozen_balance_by_cycle/");

        /// <summary>
        /// Gets the query to the cycle by the end of which the delegate might be deactivated if he fails to execute any delegate action
        /// </summary>
        public RpcObject GracePeriod => new RpcObject(this, "grace_period/");

        /// <summary>
        /// Gets the query to the total amount of tokens delegated to a given delegate, including the balance of the delegate itself and its frozen fees and deposits
        /// </summary>
        public RpcObject StakingBalance => new RpcObject(this, "staking_balance/");

        internal DelegateQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}
