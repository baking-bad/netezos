using System;

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
        [Obsolete("This RPC query was removed. Use it on early protocols only.")]
        public RpcObject Balance => new RpcObject(this, "balance/");

        /// <summary>
        /// Gets the query to the full balance of a given delegate, including the frozen balances
        /// </summary>
        public RpcObject FullBalance => new RpcObject(this, "full_balance/");
        
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
        [Obsolete("This RPC query was removed. Use it on early protocols only.")]
        public RpcObject FrozenBalance => new RpcObject(this, "frozen_balance/");

        /// <summary>
        /// Gets the query to the frozen balances of a given delegate, indexed by the cycle by which it will be unfrozen
        /// </summary>
        [Obsolete("This RPC query was removed. Use it on early protocols only.")]
        public RpcObject FrozenBalanceByCycle => new RpcObject(this, "frozen_balance_by_cycle/");
        
        /// <summary>
        /// Returns the initial amount (that is, at the beginning of a cycle) of the frozen deposits (in mutez). This amount is the same as the current amount of the frozen deposits, unless the delegate has been punished.
        /// </summary>
        public RpcObject FrozenDeposits => new RpcObject(this, "frozen_deposits/");

        /// <summary>
        /// Returns the frozen deposits limit for the given delegate or none if no limit is set.
        /// </summary>
        public RpcObject FrozenDepositsLimit => new RpcObject(this, "frozen_deposits_limit/");

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
