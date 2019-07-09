using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    public class InjectOperationQuery : RpcPost
    {
        internal InjectOperationQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Inject an operation in node and broadcast it. Returns the ID of the operation.
        /// </summary>
        /// <param name="data">Signed operation bytes of forged operation</param>
        /// <param name="async">Async(optional)</param>
        /// <param name="chain">Chain(optional)</param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(string data, bool async = false, Chain chain = Chain.Main)
            => await Client.PostJson(
                $"{Query}?async={async}&chain={chain.ToString().ToLower()}",
                $"\"{data}\"");

        /// <summary>
        /// Inject an operation in node and broadcast it. Returns the ID of the operation.
        /// </summary>
        /// <param name="data">Signed operation bytes of forged operation</param>
        /// <param name="async">Async(optional)</param>
        /// <param name="chain">Chain(optional)</param>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <returns></returns>
        public async Task<T> PostAsync<T>(string data, bool async = false, Chain chain = Chain.Main)
            => await Client.PostJson<T>(
                $"{Query}?async={async}&chain={chain.ToString().ToLower()}",
                $"\"{data}\"");
    }
}