using System.Collections.Generic;
using System.Threading.Tasks;

namespace Netezos.Rpc.Queries
{
    public class MempoolQuery : RpcQuery
    {
        internal MempoolQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        public Task<dynamic> GetAsync()
            => GetAsync();
    }
}