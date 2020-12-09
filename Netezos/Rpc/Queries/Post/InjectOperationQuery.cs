using System.Collections.Generic;
using System.Threading.Tasks;
using Netezos.Encoding;

namespace Netezos.Rpc.Queries.Post
{
    /// <summary>
    /// Rpc query to access operations injection
    /// </summary>
    public class InjectOperationQuery : RpcMethod
    {
        internal InjectOperationQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Injects an operation into the node and returns the ID of the operation
        /// </summary>
        /// <param name="data">Signed operation bytes</param>
        /// <param name="async">Async (optional)</param>
        /// <param name="chain">Chain (optional)</param>
        /// <returns></returns>
        public Task<dynamic> PostAsync(byte[] data, bool async = false, Chain chain = Chain.Main)
            => Client.PostJson(
                $"{Query}?async={async}&chain={chain.ToString().ToLower()}",
                $"\"{Hex.Convert(data)}\"");

        /// <summary>
        /// Injects an operation into the node and returns the ID of the operation
        /// </summary>
        /// <param name="data">Signed operation bytes</param>
        /// <param name="async">Async (optional)</param>
        /// <param name="chain">Chain (optional)</param>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <returns></returns>
        public Task<T> PostAsync<T>(byte[] data, bool async = false, Chain chain = Chain.Main)
            => Client.PostJson<T>(
                $"{Query}?async={async}&chain={chain.ToString().ToLower()}",
                $"\"{Hex.Convert(data)}\"");

        /// <summary>
        /// Injects an operation into the node and returns the ID of the operation
        /// </summary>
        /// <param name="data">Signed operation bytes</param>
        /// <param name="async">Async (optional)</param>
        /// <param name="chain">Chain (optional)</param>
        /// <returns></returns>
        public Task<dynamic> PostAsync(IEnumerable<byte> data, bool async = false, Chain chain = Chain.Main)
            => Client.PostJson(
                $"{Query}?async={async}&chain={chain.ToString().ToLower()}",
                $"\"{Hex.Convert(data)}\"");

        /// <summary>
        /// Injects an operation into the node and returns the ID of the operation
        /// </summary>
        /// <param name="data">Signed operation bytes</param>
        /// <param name="async">Async (optional)</param>
        /// <param name="chain">Chain (optional)</param>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <returns></returns>
        public Task<T> PostAsync<T>(IEnumerable<byte> data, bool async = false, Chain chain = Chain.Main)
            => Client.PostJson<T>(
                $"{Query}?async={async}&chain={chain.ToString().ToLower()}",
                $"\"{Hex.Convert(data)}\"");

        /// <summary>
        /// Injects an operation into the node and returns the ID of the operation
        /// </summary>
        /// <param name="data">Signed operation hex bytes</param>
        /// <param name="async">Async (optional)</param>
        /// <param name="chain">Chain (optional)</param>
        /// <returns></returns>
        public Task<dynamic> PostAsync(string data, bool async = false, Chain chain = Chain.Main)
            => Client.PostJson(
                $"{Query}?async={async}&chain={chain.ToString().ToLower()}",
                $"\"{data}\"");

        /// <summary>
        /// Injects an operation into the node and returns the ID of the operation
        /// </summary>
        /// <param name="data">Signed operation hex bytes</param>
        /// <param name="async">Async (optional)</param>
        /// <param name="chain">Chain (optional)</param>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <returns></returns>
        public Task<T> PostAsync<T>(string data, bool async = false, Chain chain = Chain.Main)
            => Client.PostJson<T>(
                $"{Query}?async={async}&chain={chain.ToString().ToLower()}",
                $"\"{data}\"");
    }
}