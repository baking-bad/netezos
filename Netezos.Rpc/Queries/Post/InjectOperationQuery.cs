using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    public class InjectOperationQuery : RpcPost
    {
        internal InjectOperationQuery(RpcQuery baseQuery, string append) : base(baseQuery, append)
        {
        }

        /// <summary>Inject operation</summary>
        /// <param name="data">Data</param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(string data)
        {
            return await base.PostAsync(data);
        }
    }
}