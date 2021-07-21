using System.Collections.Generic;
using System.Threading.Tasks;

namespace Netezos.Rpc.Queries
{
    public class MempoolQuery : RpcQuery
    {
        internal MempoolQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        public RpcObject PendingOperations => new RpcObject(this, "pending_operations/");
        public RpcObject MonitorOperations => new RpcObject(this, "monitor_operations/");
    }
}