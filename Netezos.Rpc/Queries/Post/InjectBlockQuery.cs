using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    public class InjectBlockQuery : RpcPost
    {
        internal InjectBlockQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Inject a block in the node and broadcast it. Returns the ID of the block.
        /// </summary>
        /// <param name="data">Forged block header data</param>
        /// <param name="operations">List of operations</param>
        /// <param name="async">Async(optional)</param>
        /// <param name="force">Force(optional)</param>
        /// <param name="chain">Chain(optional)</param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(string data, List<List<object>> operations,bool async = false, bool force = false, Chain chain = Chain.Main)
            => await Client.PostJson(
                $"{Query}?async={async}&force={force}&chain={chain.ToString().ToLower()}",
                new { data, operations }.ToJson());

        /// <summary>
        /// Inject a block in the node and broadcast it. Returns the ID of the block.
        /// </summary>
        /// <param name="data">Forged block header data</param>
        /// <param name="operations">List of operations</param>
        /// <param name="async">Async(optional)</param>
        /// <param name="force">Force(optional)</param>
        /// <param name="chain">Chain(optional)</param>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <returns></returns>
        public async Task<T> PostAsync<T>(string data, List<List<object>> operations, bool async = false, bool force = false, Chain chain = Chain.Main)
            => await Client.PostJson<T>(
                $"{Query}?async={async}&force={force}&chain={chain.ToString().ToLower()}",
                new { data, operations }.ToJson());
    }
}