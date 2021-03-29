namespace Netezos.Rpc.Queries
{
    public class RawBigMapQuery : RpcObject
    {
        public RpcDictionary<string, RpcObject> Contents
            => new RpcDictionary<string, RpcObject>(this, "contents/");

        internal RawBigMapQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}
