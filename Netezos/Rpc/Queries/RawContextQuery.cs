using System;

namespace Netezos.Rpc.Queries
{
    public class RawContextQuery : DeepRpcObject
    {
        public DeepRpcDictionary<string, RpcObject> ActiveDelegates
            => new DeepRpcDictionary<string, RpcObject>(this, "active_delegates_with_rolls/");

        public DeepRpcDictionary<string, RpcObject> Commitments
            => new DeepRpcDictionary<string, RpcObject>(this, "commitments/");

        public RpcObject GlobalCounter
            => new RpcObject(this, "contracts/global_counter/");

        public DeepRpcDictionary<string, RawContractQuery> Contracts
            => new DeepRpcDictionary<string, RawContractQuery>(this, "contracts/index/");

        public DeepRpcDictionary<int, RawCycleQuery> Cycles
            => new DeepRpcDictionary<int, RawCycleQuery>(this, "cycle/");

        public DeepRpcDictionary<string, RpcObject> Delegates
            => new DeepRpcDictionary<string, RpcObject>(this, "delegates/");

        public DeepRpcDictionary<int, RpcDictionary<string, RpcObject>> DelegatesWithFrozenBalance
            => new DeepRpcDictionary<int, RpcDictionary<string, RpcObject>>(this, "delegates_with_frozen_balance/");

        [Obsolete("This RPC query was removed. Use it on early protocols only.")]
        public RpcObject LastBlockPriority
            => new RpcObject(this, "last_block_priority/");

        // TODO: describe this
        public DeepRpcObject RampUp => new DeepRpcObject(this, "ramp_up/");

        public RawRollsQuery Rolls => new RawRollsQuery(this, "rolls/");

        public RawVotesQuery Votes => new RawVotesQuery(this, "votes/");

        internal RawContextQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}
