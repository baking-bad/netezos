namespace Netezos.Rpc.Queries
{
    public class RawRollQuery : RpcObject
    {
        public RpcObject Successor => new RpcObject(this, "successor/");

        internal RawRollQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}
