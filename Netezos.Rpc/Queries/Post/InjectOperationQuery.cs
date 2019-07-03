using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    //TODO fix xml docs
    public class InjectOperationQuery : RpcPost
    {
        internal InjectOperationQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Inject block query
        /// </summary>
        /// <param name="data">Data</param>
        /// <param name="async">Async</param>
        /// <param name="chain">Chain</param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(string data, bool async = false, Chain chain = Chain.Main)
            => await Client.PostJson(
                $"{Query}?async={async}&chain={chain.ToString().ToLower()}",
                $"\"{data}\"");

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="async"></param>
        /// <param name="chain"></param>
        /// <returns></returns>
        public async Task<T> PostAsync<T>(string data, bool async = false, Chain chain = Chain.Main)
            => await Client.PostJson<T>(
                $"{Query}?async={async}&chain={chain.ToString().ToLower()}",
                $"\"{data}\"");
    }
}