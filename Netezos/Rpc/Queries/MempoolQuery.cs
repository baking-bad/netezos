namespace Netezos.Rpc.Queries
{
    public class MempoolQuery : RpcQuery
    {
        internal MempoolQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        public RpcObject PendingOperations => new RpcObject(this, "pending_operations/");

        // this is long-polling, that should be handled in a different way
        //public RpcObject MonitorOperations => new RpcObject(this, "monitor_operations/");
    }
}