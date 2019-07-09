using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    public class ForgeOperationsQuery : RpcPost
    {
        internal ForgeOperationsQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Forge an operation and returns operation bytes which can be signed
        /// </summary>
        /// <param name="branch">Branch</param>
        /// <param name="contents">List of operation contents</param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(string branch, List<object> contents)
            => await PostAsync(new
            {
                branch,
                contents
            });

        /// <summary>
        /// Forge an operation and returns operation bytes which can be signed
        /// </summary>
        /// <param name="branch">Branch</param>
        /// <param name="contents">List of operation contents</param>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <returns></returns>
        public async Task<T> PostAsync<T>(string branch, List<object> contents)
            => await PostAsync<T>(new
            {
                branch,
                contents
            });
    }
}