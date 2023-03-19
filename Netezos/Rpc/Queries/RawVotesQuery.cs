namespace Netezos.Rpc.Queries
{
    public class RawVotesQuery : DeepRpcObject
    {
        public DeepRpcDictionary<string, RpcObject> Ballots => new(this, "ballots/");

        [Obsolete("This RPC query was removed. Use it on early protocols only.")]
        public RpcObject CurrentPeriodKind => new(this, "current_period_kind/");

        public RpcObject CurrentProposal => new(this, "current_proposal/");

        public RpcObject CurrentQuorum => new(this, "current_quorum/");

        public DeepRpcDictionary<string, RpcObject> Listings => new(this, "listings/");

        [Obsolete("This RPC query was removed. Use it on early protocols only.")]
        public RpcObject ListingsSize => new(this, "listings_size/");

        public DeepRpcDictionary<string, DeepRpcDictionary<string, RpcObject>> Proposals => new(this, "proposals/");

        public DeepRpcDictionary<string, RpcObject> ProposalsCount => new(this, "proposals_count/");

        internal RawVotesQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}
