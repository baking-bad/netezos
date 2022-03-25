using System;

namespace Netezos.Rpc.Queries
{
    /// <summary>
    /// Rpc query to access contract data
    /// </summary>
    public class DelegateQuery : RpcObject
    {
        /// <summary>
        /// Returns the current amount of the frozen deposits (in mutez).
        /// </summary>
        public RpcObject CurrentFrozenDeposits => new RpcObject(this, "current_frozen_deposits/");
        
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
        /// Returns cycle and level participation information. In particular this indicates, in the field 'expected_cycle_activity',
        /// the number of slots the delegate is expected to have in the cycle based on its active stake. The field 'minimal_cycle_activity' indicates the minimal endorsing slots in the cycle required to get endorsing rewards.
        /// It is computed based on 'expected_cycle_activity. The fields 'missed_slots' and 'missed_levels' indicate the number of missed endorsing slots and missed levels (for endorsing) in the cycle so far.
        /// 'missed_slots' indicates the number of missed endorsing slots in the cycle so far. The field 'remaining_allowed_missed_slots' indicates the remaining amount of endorsing slots that can be missed in the cycle before forfeiting the rewards.
        /// Finally, 'expected_endorsing_rewards' indicates the endorsing rewards that will be distributed at the end of the cycle if activity at that point will be greater than the minimal required; if the activity is already known to be below the required minimum, then the rewards are zero.
        /// </summary>
        public RpcObject Participation => new RpcObject(this, "participation/");

        /// <summary>
        /// Gets the query to the total amount of tokens delegated to a given delegate, including the balance of the delegate itself and its frozen fees and deposits
        /// </summary>
        public RpcObject StakingBalance => new RpcObject(this, "staking_balance/");

        /// <summary>
        /// The number of rolls in the vote listings for a given delegate
        /// </summary>
        public RpcObject VotingPower => new RpcObject(this, "voting_power/");

        internal DelegateQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}
