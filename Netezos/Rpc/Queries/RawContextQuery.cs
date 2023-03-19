namespace Netezos.Rpc.Queries
{
    public class RawContextQuery : DeepRpcObject
    {
        [Obsolete("This RPC query was removed. Use it on early protocols only.")]
        public DeepRpcDictionary<string, RpcObject> ActiveDelegates => new(this, "active_delegates_with_rolls/");

        public RpcArray<RawBigMapQuery> BigMaps => new(this, "big_maps/index/");

        public DeepRpcDictionary<string, RpcObject> Commitments => new(this, "commitments/");

        public RpcObject GlobalCounter => new(this, "contracts/global_counter/");

        public DeepRpcDictionary<string, RawContractQuery> Contracts => new(this, "contracts/index/");

        public DeepRpcDictionary<int, RawCycleQuery> Cycles => new(this, "cycle/");
        
        public DeepRpcDictionary<string, RpcObject> Delegates => new(this, "delegates/");

        [Obsolete("This RPC query was removed. Use it on early protocols only.")]
        public DeepRpcDictionary<int, RpcDictionary<string, RpcObject>> DelegatesWithFrozenBalance => new(this, "delegates_with_frozen_balance/");

        [Obsolete("This RPC query was removed. Use it on early protocols only.")]
        public RpcObject LastBlockPriority => new(this, "last_block_priority/");

        // TODO: describe this
        public DeepRpcObject RampUp => new(this, "ramp_up/");

        [Obsolete("This RPC query was removed. Use it on early protocols only.")]
        public RawRollsQuery Rolls => new(this, "rolls/");

        public RawVotesQuery Votes => new(this, "votes/");

        internal RawContextQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}
