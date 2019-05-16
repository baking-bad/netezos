namespace Netezos.Rpc.Queries
{
    public class RawRollsQuery : DeepRpcObject
    {
        public RpcObject Limbo => new RpcObject(this, "limbo/");

        public RpcObject Next => new RpcObject(this, "next/");

        public DeepRpcDictionary<int, RawRollQuery> Index
            => new DeepRpcDictionary<int, RawRollQuery>(this, "index/");

        public DeepRpcDictionary<int, RpcObject> OwnerCurrent
            => new DeepRpcDictionary<int, RpcObject>(this, "owner/current/");

        public DeepRpcDictionary<int, DeepRpcDictionary<int, DeepRpcDictionary<int, RpcObject>>> OwnerSnapshot
            => new DeepRpcDictionary<int, DeepRpcDictionary<int, DeepRpcDictionary<int, RpcObject>>>(this, "owner/snapshot/");


        internal RawRollsQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}
