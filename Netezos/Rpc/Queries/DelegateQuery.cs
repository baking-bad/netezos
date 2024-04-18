namespace Netezos.Rpc.Queries
{
    /// <summary>
    /// Rpc query to access contract data
    /// </summary>
    public class DelegateQuery : RpcObject
    {
        /// <summary>
        /// Returns the currently active staking parameters for the given delegate.
        /// </summary>
        public RpcObject ActiveStakingParameters => new(this, "active_staking_parameters/");
        
        /// <summary>
        /// Gets the query to the full balance of a given delegate, including the frozen balances
        /// </summary>
        [Obsolete("This RPC query was removed. Use it on early protocols only.")]
        public RpcObject Balance => new(this, "balance/");
        
        /// <summary>
        /// The active consensus key for a given delegate and the pending consensus keys.
        /// </summary>
        public RpcObject ConsensusKey => new(this, "consensus_key/");
        
        /// <summary>
        /// The baking power of a delegate, as computed from its current stake. This value is not used for computing baking
        /// rights but only reflects the baking power that the delegate would have if the cycle ended at the current block. 
        /// </summary>
        public RpcObject CurrentBakingPower => new(this, "current_baking_power/");
        
        /// <summary>
        /// Returns the current amount of the frozen deposits (in mutez).
        /// </summary>
        public RpcObject CurrentFrozenDeposits => new(this, "current_frozen_deposits/");
        
        /// <summary>
        /// The voting power of a given delegate, as computed from its current stake.
        /// </summary>
        public RpcObject CurrentVotingPower => new(this, "current_voting_power/");
        
        /// <summary>
        /// Gets the query to the flag, indicating whether the delegate is currently tagged as deactivated or not
        /// </summary>
        public RpcObject Deactivated => new(this, "deactivated/");

        /// <summary>
        /// Gets the query to the balances of all the contracts that delegate to a given delegate. This excludes the delegate's own balance and its frozen balances
        /// </summary>
        public RpcObject DelegatedBalance => new(this, "delegated_balance/");

        /// <summary>
        /// Gets the query to the list of contracts that delegate to a given delegate
        /// </summary>
        public RpcObject DelegatedContracts => new(this, "delegated_contracts/");

        /// <summary>
        /// Gets the query to the pending denunciations for the given delegate.
        /// </summary>
        public RpcObject Denunciations => new(this, "denunciations/");

        /// <summary>
        /// Gets the query to the estimated shared pending slashed amount (in mutez) of a given delegate.
        /// </summary>
        public RpcObject EstimatedSharedPendingSlashedAmount => new(this, "estimated_shared_pending_slashed_amount/");

        /// <summary>
        /// Gets the query to the total frozen balances of a given delegate, this includes the frozen deposits, rewards and fees
        /// </summary>
        [Obsolete("This RPC query was removed. Use it on early protocols only.")]
        public RpcObject FrozenBalance => new(this, "frozen_balance/");

        /// <summary>
        /// Gets the query to the frozen balances of a given delegate, indexed by the cycle by which it will be unfrozen
        /// </summary>
        [Obsolete("This RPC query was removed. Use it on early protocols only.")]
        public RpcObject FrozenBalanceByCycle => new(this, "frozen_balance_by_cycle/");

        /// <summary>
        /// Returns the initial amount (that is, at the beginning of a cycle) of the frozen deposits (in mutez). This amount is the same as the current amount of the frozen deposits, unless the delegate has been punished.
        /// </summary>
        public RpcObject FrozenDeposits => new(this, "frozen_deposits/");

        /// <summary>
        /// Returns the frozen deposits limit for the given delegate or none if no limit is set.
        /// </summary>
        [Obsolete("This RPC query was removed. Use it on early protocols only.")]
        public RpcObject FrozenDepositsLimit => new(this, "frozen_deposits_limit/");

        /// <summary>
        /// Gets the query to the full balance of a given delegate, including the frozen balances
        /// </summary>
        public RpcObject FullBalance => new(this, "full_balance/");
        
        /// <summary>
        /// Gets the query to the cycle by the end of which the delegate might be deactivated if he fails to execute any delegate action
        /// </summary>
        public RpcObject GracePeriod => new(this, "grace_period/");

        /// <summary>
        /// Gets the query that returns true if the delegate is forbidden to participate in consensus.
        /// </summary>
        public RpcObject IsForbidden => new(this, "is_forbidden/");

        /// <summary>
        /// Gets the query to the minimum of delegated tez (in mutez) over the current cycle and the block level where
        /// this value was last updated. 
        /// </summary>
        public RpcObject MinDelegatedInCurrentCycle => new(this, "min_delegated_in_current_cycle/");

        /// <summary>
        /// Returns cycle and level participation information. In particular this indicates, in the field 'expected_cycle_activity',
        /// the number of slots the delegate is expected to have in the cycle based on its active stake. The field 'minimal_cycle_activity' indicates the minimal endorsing slots in the cycle required to get endorsing rewards.
        /// It is computed based on 'expected_cycle_activity. The fields 'missed_slots' and 'missed_levels' indicate the number of missed endorsing slots and missed levels (for endorsing) in the cycle so far.
        /// 'missed_slots' indicates the number of missed endorsing slots in the cycle so far. The field 'remaining_allowed_missed_slots' indicates the remaining amount of endorsing slots that can be missed in the cycle before forfeiting the rewards.
        /// Finally, 'expected_endorsing_rewards' indicates the endorsing rewards that will be distributed at the end of the cycle if activity at that point will be greater than the minimal required; if the activity is already known to be below the required minimum, then the rewards are zero.
        /// </summary>
        public RpcObject Participation => new(this, "participation/");

        /// <summary>
        /// Returns the pending values for the given delegate's staking parameters.
        /// </summary>
        public RpcObject PendingStakingParameters => new(this, "pending_staking_parameters/");

        /// <summary>
        /// Gets the query to the total amount of tokens delegated to a given delegate, including the balance of the delegate itself and its frozen fees and deposits
        /// </summary>
        public RpcObject StakingBalance => new(this, "staking_balance/");

        /// <summary>
        /// Returns, for each cycle, the sum of unstaked-but-frozen deposits for this cycle. Cycles go from the last unslashable cycle to the current cycle.
        /// </summary>
        public RpcObject UnstakedFrozenDeposits => new(this, "unstaked_frozen_deposits/");

        /// <summary>
        /// The number of rolls in the vote listings for a given delegate
        /// </summary>
        public RpcObject VotingInfo => new(this, "voting_info/");
        
        /// <summary>
        /// The number of rolls in the vote listings for a given delegate
        /// </summary>
        public RpcObject VotingPower => new(this, "voting_power/");

        internal DelegateQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}
