using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    public class InjectOperationQuery : RpcPost
    {
        internal InjectOperationQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) {}
        /// <summary>Inject block query</summary>
        /// <param name="data">Data</param>
        /// <param name="async">Async</param>
        /// <param name="chain">Chain</param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(string data, bool async = false, string chain = "main")
            => await Client.Post($"{Query}?async={async}&chain={chain}", $"\"{data}\"");
    }
}