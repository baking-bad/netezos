using System.Collections.Generic;
using System.Threading.Tasks;

namespace Netezos.Rpc.Queries.Post
{
    /// <summary>
    /// Rpc query to access blocks injection
    /// </summary>
    public class InjectBlockQuery : RpcMethod
    {
        internal InjectBlockQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Injects a block into the node and returns the ID of the block
        /// </summary>
        /// <param name="data">Forged block header data</param>
        /// <param name="operations">List of operations</param>
        /// <param name="async">Async (optional)</param>
        /// <param name="force">Force (optional)</param>
        /// <param name="chain">Chain (optional)</param>
        /// <returns></returns>
        public Task<dynamic> PostAsync(string data, List<List<object>> operations, bool async = false, bool force = false, Chain chain = Chain.Main)
            => Client.PostJson(
                $"{Query}?async={async}&force={force}&chain={chain.ToString().ToLower()}",
                new { data, operations });

        /// <summary>
        /// Injects a block into the node and returns the ID of the block
        /// </summary>
        /// <param name="data">Forged block header data</param>
        /// <param name="operations">List of operations</param>
        /// <param name="async">Async (optional)</param>
        /// <param name="force">Force (optional)</param>
        /// <param name="chain">Chain (optional)</param>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <returns></returns>
        public Task<T> PostAsync<T>(string data, List<List<object>> operations, bool async = false, bool force = false, Chain chain = Chain.Main)
            => Client.PostJson<T>(
                $"{Query}?async={async}&force={force}&chain={chain.ToString().ToLower()}",
                new { data, operations });
    }
}