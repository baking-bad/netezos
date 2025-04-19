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
        /// Returns the current baking power of a delegate, using the current staked and delegated balances of the baker
        /// and its delegators. In other words, the baking rights that the baker would get for a future cycle if the
        /// current cycle ended right at the current block.
        /// </summary>
        public RpcObject BakingPower => new(this, "baking_power/");

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
        [Obsolete("This RPC query was removed. Use it on early protocols only.")]
        public RpcObject CurrentBakingPower => new(this, "current_baking_power/");

        /// <summary>
        /// Returns the current amount of the frozen deposits (in mutez).
        /// </summary>
        [Obsolete("This RPC query was removed. Use it on early protocols only.")]
        public RpcObject CurrentFrozenDeposits => new(this, "current_frozen_deposits/");

        /// <summary>
        /// The voting power of a given delegate, as computed from its current stake.
        /// </summary>
        public RpcObject CurrentVotingPower => new(this, "current_voting_power/");
        
        /// <summary>
        /// Returns information about the delegate's participation in the attestation of slots published into the Data
        /// Availability Layer (DAL) during the current cycle.
        /// </summary>
        public RpcObject DalParticipation => new(this, "dal_participation/");

        /// <summary>
        /// Gets the query to the flag, indicating whether the delegate is currently tagged as deactivated or not
        /// </summary>
        public RpcObject Deactivated => new(this, "deactivated/");

        /// <summary>
        /// Gets the query to the balances of all the contracts that delegate to a given delegate. This excludes the delegate's own balance and its frozen balances
        /// </summary>
        [Obsolete("This RPC query was removed. Use it on early protocols only.")]
        public RpcObject DelegatedBalance => new(this, "delegated_balance/");

        /// <summary>
        /// Gets the query to the list of contracts that delegate to a given delegate
        /// </summary>
        [Obsolete("This RPC query was removed. Use it on early protocols only.")]
        public RpcObject DelegatedContracts => new(this, "delegated_contracts/");
        
        /// <summary>
        /// Gets the query to the list of all contracts that are currently delegating to the delegate. Includes both
        /// user accounts and smart contracts. Includes the delegate itself.
        /// </summary>
        public RpcObject Delegators => new(this, "delegators/");

        /// <summary>
        /// Gets the query to the pending denunciations for the given delegate.
        /// </summary>
        public RpcObject Denunciations => new(this, "denunciations/");

        /// <summary>
        /// Gets the query to the estimated shared pending slashed amount (in mutez) of a given delegate.
        /// </summary>
        public RpcObject EstimatedSharedPendingSlashedAmount => new(this, "estimated_shared_pending_slashed_amount/");
        
        /// <summary>
        /// Gets the query to the sum (in mutez) of non-staked tokens that currently count as delegated to the baker,
        /// excluding those owned by the baker iself. Does not take limits such as overstaking or overdelegation into
        /// account. This includes the spendable balances and frozen bonds of all the baker's external delegators. It
        /// also includes unstake requests of contracts other than the baker, on the condition that the contract was
        /// delegating to the baker at the time of the unstake operation. So this includes most but not all unstake
        /// requests from current delegators, and might include some unstake requests from old delegators. Limits such
        /// as overstaking and overdelegation have not been applied yet.
        /// </summary>
        public RpcObject ExternalDelegated => new(this, "external_delegated/");
        
        /// <summary>
        /// Gets the query to the sum (in mutez) of all tokens currently staked by the baker's external delegators. This
        /// excludes the baker's own staked tokens.
        /// </summary>
        public RpcObject ExternalStaked => new(this, "external_staked/");

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
        /// Returns the initial amount (that is, at the beginning of a cycle) of the frozen deposits (in mutez). This
        /// amount is the same as the current amount of the frozen deposits, unless the delegate has been punished.
        /// </summary>
        [Obsolete("This RPC query was removed. Use it on early protocols only.")]
        public RpcObject FrozenDeposits => new(this, "frozen_deposits/");

        /// <summary>
        /// Returns the frozen deposits limit for the given delegate or none if no limit is set.
        /// </summary>
        [Obsolete("This RPC query was removed. Use it on early protocols only.")]
        public RpcObject FrozenDepositsLimit => new(this, "frozen_deposits_limit/");

        /// <summary>
        /// Gets the query to the full balance of a given delegate, including the frozen balances
        /// </summary>
        [Obsolete("This RPC query was removed. Use it on early protocols only.")]
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
        /// Gets the query to the amount (in mutez) currently owned by the baker itself and counting as delegated for
        /// the purpose of baking rights. This corresponds to all non-staked tokens owned by the baker: spendable
        /// balance, frozen bonds, and unstake requests. (Note: There is one exception: if the baker still has unstake
        /// requests created at a time when it was delegating to a different delegate, then these unstake requests still
        /// count as delegated to the former delegate. Any such unstake requests are excluded from the amount returned
        /// by the present RPC, despite being non-staked tokens owned by the baker.)
        /// </summary>
        public RpcObject OwnDelegated => new(this, "own_delegated/");
        
        /// <summary>
        /// Gets the query to the full balance (in mutez) of tokens owned by the delegate itself. Includes its spendable
        /// balance, staked tez, unstake requests, and frozen bonds. Does not include any tokens owned by external
        /// delegators. This RPC fails when the pkh is not a delegate. When it is a delegate, this RPC outputs the same
        /// amount as ../:block_id/context/contracts/:delegate_contract_id/full_balance.
        /// </summary>
        public RpcObject OwnFullBalance => new(this, "own_full_balance/");
        
        /// <summary>
        /// Gets the query to the amount (in mutez) currently owned and staked by the baker itself. Returns the same
        /// value as ../:block_id/context/contracts/:delegate_contract_id/staked_balance (except for the fact that the
        /// present RPC fails if the public_key_hash in the path is not a delegate).
        /// </summary>
        public RpcObject OwnStaked => new(this, "own_staked/");

        /// <summary>
        /// Returns cycle and level participation information. In particular this indicates, in the field 'expected_cycle_activity',
        /// the number of slots the delegate is expected to have in the cycle based on its active stake. The field 'minimal_cycle_activity' indicates the minimal attesting slots in the cycle required to get attesting rewards.
        /// It is computed based on 'expected_cycle_activity. The fields 'missed_slots' and 'missed_levels' indicate the number of missed attesting slots and missed levels (for attesting) in the cycle so far.
        /// 'missed_slots' indicates the number of missed attesting slots in the cycle so far. The field 'remaining_allowed_missed_slots' indicates the remaining amount of attesting slots that can be missed in the cycle before forfeiting the rewards.
        /// Finally, 'expected_attesting_rewards' indicates the attesting rewards that will be distributed at the end of the cycle if activity at that point will be greater than the minimal required; if the activity is already known to be below the required minimum, then the rewards are zero.
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
        /// Gets the query to the all tokens (in mutez) that currently count as delegated for the purpose of computing
        /// the baker's rights; they weigh half as much as staked tez in the rights. Limits such as overstaking and
        /// overdelegation have not been applied yet. This corresponds to all non-staked tez owned by the baker's
        /// delegators (including the baker itself): spendable balances, frozen bonds, and unstaked requests, except for
        /// any unstake requests that have been created before the delegator changed its delegate to the current baker
        /// (because they still count as delegated for the old delegate instead).
        /// </summary>
        public RpcObject TotalDelegated => new(this, "total_delegated/");
        
        /// <summary>
        /// Gets the query to the total amount (in mutez) currently staked for the baker, both by the baker itself and
        /// by external stakers. This is the staked amount before applying the baker's 'limit_of_staking_over_baking';
        /// in other words, it includes overstaked tez if there are any.
        /// </summary>
        public RpcObject TotalStaked => new(this, "total_staked/");
        
        /// <summary>
        /// Gets the query that returns for each cycle the total amount (in mutez) contained in all unstake requests
        /// created during this cycle by all delegators, including the baker itself. Note that these tokens count as
        /// delegated to the baker for the purpose of computing baking rights, and are included in the amount returned
        /// by the total_delegated RPC.
        /// </summary>
        public RpcObject TotalUnstakedPerCycle => new(this, "total_unstaked_per_cycle/");

        /// <summary>
        /// Returns, for each cycle, the sum of unstaked-but-frozen deposits for this cycle. Cycles go from the last unslashable cycle to the current cycle.
        /// </summary>
        [Obsolete("This RPC query was removed. Use it on early protocols only.")]
        public RpcObject UnstakedFrozenDeposits => new(this, "unstaked_frozen_deposits/");

        /// <summary>
        /// The number of rolls in the vote listings for a given delegate
        /// </summary>
        public RpcObject VotingInfo => new(this, "voting_info/");

        /// <summary>
        /// The number of rolls in the vote listings for a given delegate
        /// </summary>
        public RpcObject VotingPower => new(this, "voting_power/");

        internal DelegateQuery(RpcQuery baseQuery, string append) : base(baseQuery, append)
        {
        }
    }
}