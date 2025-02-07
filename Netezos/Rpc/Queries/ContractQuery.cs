﻿using Netezos.Rpc.Queries.Post;

namespace Netezos.Rpc.Queries
{
    /// <summary>
    /// Rpc query to access contract data
    /// </summary>
    public class ContractQuery : RpcObject
    {
        /// <summary>
        /// Gets the query to the complete list of tickets owned by the given contract by scanning the contract's storage.
        /// </summary>
        public RpcObject AllTicketBalances => new(this, "all_ticket_balances/");
        
        /// <summary>
        /// Gets the query to the spendable balance of a contract (in mutez), also known as liquid balance. Corresponds
        /// to tez owned by the contract that are neither staked, nor in unstaked requests, nor in frozen bonds.
        /// Identical to the 'spendable' RPC.
        /// </summary>
        public RpcObject Balance => new(this, "balance/");
        
        /// <summary>
        /// Gets the query to the sum (in mutez) of the spendable balance and frozen bonds of a contract. Corresponds
        /// to the contract's full balance from which staked funds and unstake requests have been excluded. Identical
        /// to the 'spendable_and_frozen_bonds' RPC.
        /// </summary>
        public RpcObject BalanceAndFrozenBonds => new(this, "balance_and_frozen_bonds/");

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
        /// Gets the query to the estimated own pending slashed amount (in mutez) of a contract.
        /// </summary>
        public RpcObject EstimatedOwnPendingSlashedAmount => new(this, "estimated_own_pending_slashed_amount/");

        /// <summary>
        /// Gets the query to the frozen bonds of a contract.
        /// </summary>
        public RpcObject FrozenBonds => new(this, "frozen_bonds/");

        /// <summary>
        /// Gets the query to the full balance (in mutez) of the contract. Includes its spendable balance, staked tez,
        /// unstake requests, and frozen bonds. Even if the contract is a delegate, it does not include any staked or
        /// delegated tez owned by external delegators.
        /// </summary>
        public RpcObject FullBalance => new(this, "full_balance/");

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
        /// Gets the query to the staked balance of a contract (in mutez). Returns None if the contract is originated,
        /// or neither delegated nor a delegate.
        /// </summary>
        public RpcObject StakedBalance => new(this, "staked_balance/");

        /// <summary>
        /// Gets the query to the spendable balance of a contract (in mutez), also known as liquid balance. Corresponds
        /// to tez owned by the contract that are neither staked, nor in unstaked requests, nor in frozen bonds.
        /// Identical to the 'balance' RPC.
        /// </summary>
        public RpcObject Spendable => new(this, "spendable/");
        
        /// <summary>
        /// Gets the query to the sum (in mutez) of the spendable balance and frozen bonds of a contract. Corresponds
        /// to the contract's full balance from which staked funds and unstake requests have been excluded. Identical
        /// to the 'balance_and_frozen_bonds' RPC.
        /// </summary>
        public RpcObject SpendableAndFrozenBonds => new(this, "spendable_and_frozen_bonds/");

        /// <summary>
        /// Gets the query to the data of the contract's storage
        /// </summary>
        public StorageQuery Storage => new(this, "storage/");

        /// <summary>
        /// Gets the contract's balance of ticket with specified ticketer, content type, and content.
        /// </summary>
        public TicketBalanceQuery TicketBalance => new(this, "ticket_balance/");

        /// <summary>
        /// Gets the unstake requests of the contract. The requests that appear in the finalizable field can be finalized,
        /// which means that the contract can transfer these (no longer frozen) funds to their spendable balance with a [finalize_unstake] operation call.
        /// </summary>
        public RpcObject UnstakeRequests => new(this, "unstake_requests/");

        /// <summary>
        /// Gets the balance of a contract that was requested for an unstake operation, and is no longer frozen,
        /// which means it will appear in the spendable balance of the contract after any stake/unstake/finalize_unstake operation.
        /// Returns None if the contract is originated.
        /// </summary>
        public RpcObject UnstakedFinalizableBalance => new(this, "unstaked_finalizable_balance/");

        /// <summary>
        /// Gets the balance of a contract that was requested for an unstake operation, but is still frozen for the duration of the slashing period.
        /// Returns None if the contract is originated.
        /// </summary>
        public RpcObject UnstakedFrozenBalance => new(this, "unstaked_frozen_balance/");

        internal ContractQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}
