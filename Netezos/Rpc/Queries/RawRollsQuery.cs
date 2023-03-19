namespace Netezos.Rpc.Queries
{
    [Obsolete("This RPC query was removed. Use it on early protocols only.")]
    public class RawRollsQuery : DeepRpcObject
    {
        public RpcObject Limbo => new(this, "limbo/");

        public RpcObject Next => new(this, "next/");

        public DeepRpcDictionary<int, RawRollQuery> Index => new(this, "index/");

        public DeepRpcDictionary<int, RpcObject> OwnerCurrent => new(this, "owner/current/");

        public DeepRpcDictionary<int, DeepRpcDictionary<int, DeepRpcDictionary<int, RpcObject>>> OwnerSnapshot => new(this, "owner/snapshot/");

        internal RawRollsQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}
