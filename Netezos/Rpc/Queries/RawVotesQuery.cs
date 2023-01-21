using System;

namespace Netezos.Rpc.Queries
{
    public class RawVotesQuery : DeepRpcObject
    {
        public DeepRpcDictionary<string, RpcObject> Ballots
            => new DeepRpcDictionary<string, RpcObject>(this, "ballots/");
        
        [Obsolete("This RPC query was removed. Use it on early protocols only.")]
        public RpcObject CurrentPeriodKind => new RpcObject(this, "current_period_kind/");

        public RpcObject CurrentProposal => new RpcObject(this, "current_proposal/");

        public RpcObject CurrentQuorum => new RpcObject(this, "current_quorum/");

        public DeepRpcDictionary<string, RpcObject> Listings
            => new DeepRpcDictionary<string, RpcObject>(this, "listings/");

        [Obsolete("This RPC query was removed. Use it on early protocols only.")]
        public RpcObject ListingsSize => new RpcObject(this, "listings_size/");

        public DeepRpcDictionary<string, DeepRpcDictionary<string, RpcObject>> Proposals
            => new DeepRpcDictionary<string, DeepRpcDictionary<string, RpcObject>>(this, "proposals/");

        public DeepRpcDictionary<string, RpcObject> ProposalsCount
            => new DeepRpcDictionary<string, RpcObject>(this, "proposals_count/");

        internal RawVotesQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}
