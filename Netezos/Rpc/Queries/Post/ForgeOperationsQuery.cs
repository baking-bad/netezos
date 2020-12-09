using System.Collections.Generic;
using System.Threading.Tasks;

namespace Netezos.Rpc.Queries.Post
{
    /// <summary>
    /// Rpc query to access operations forging
    /// </summary>
    public class ForgeOperationsQuery : RpcMethod
    {
        internal ForgeOperationsQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Forges an operation and returns operation bytes which can be signed
        /// </summary>
        /// <param name="branch">Branch</param>
        /// <param name="contents">List of operation contents</param>
        /// <returns></returns>
        public Task<dynamic> PostAsync(string branch, List<object> contents)
            => PostAsync(new
            {
                branch,
                contents
            });

        /// <summary>
        /// Forges an operation and returns operation bytes which can be signed
        /// </summary>
        /// <param name="branch">Branch</param>
        /// <param name="contents">List of operation contents</param>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <returns></returns>
        public Task<T> PostAsync<T>(string branch, List<object> contents)
            => PostAsync<T>(new
            {
                branch,
                contents
            });
    }
}