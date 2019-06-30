using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    public class InjectBlockQuery : RpcPost
    {
        internal InjectBlockQuery(RpcQuery baseQuery, string append) : base(baseQuery, append)
        {
        }

        /// <summary>Inject block query</summary>
        /// <param name="data">Data</param>
        /// <param name="operations">List of operations</param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(string data, List<List<object>> operations)
        {
            return await PostAsync(new
            {
                data,
                operations
            });
        }
    }
}