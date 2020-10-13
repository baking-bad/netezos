using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    /// <summary>
    /// Rpc query to access operations forging
    /// </summary>
    public class ForgeOperationsQuery : RpcPost
    {
        internal ForgeOperationsQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Forges an operation and returns operation bytes which can be signed
        /// </summary>
        /// <param name="branch">Branch</param>
        /// <param name="contents">List of operation contents</param>
        /// <returns></returns>
        public Task<JToken> PostAsync(string branch, List<object> contents)
            => PostAsync(new
            {
                branch,
                contents
            });

        public override Task<JToken> PostAsync(string contents)
            => base.PostAsync(contents);

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