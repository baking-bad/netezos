using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    //TODO fix xml docs
    public class InjectBlockQuery : RpcPost
    {
        internal InjectBlockQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Inject block query
        /// </summary>
        /// <param name="data">Data</param>
        /// <param name="operations">List of operations</param>
        /// <param name="async">Async</param>
        /// <param name="force">Force</param>
        /// <param name="chain">Chain</param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(string data, List<List<object>> operations,bool async = false, bool force = false, Chain chain = Chain.Main)
            => await Client.PostJson(
                $"{Query}?async={async}&force={force}&chain={chain.ToString().ToLower()}",
                new { data, operations }.ToJson());

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="operations"></param>
        /// <param name="async"></param>
        /// <param name="force"></param>
        /// <param name="chain"></param>
        /// <returns></returns>
        public async Task<T> PostAsync<T>(string data, List<List<object>> operations, bool async = false, bool force = false, Chain chain = Chain.Main)
            => await Client.PostJson<T>(
                $"{Query}?async={async}&force={force}&chain={chain.ToString().ToLower()}",
                new { data, operations }.ToJson());
    }
}