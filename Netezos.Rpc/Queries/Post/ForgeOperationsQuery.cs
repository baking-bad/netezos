using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    //TODO fix xml docs
    public class ForgeOperationsQuery : RpcPost
    {
        internal ForgeOperationsQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Forge a Proposals operation
        /// </summary>
        /// <param name="branch">Branch</param>
        /// <param name="contents">List of contents</param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(string branch, List<object> contents)
            => await PostAsync(new
            {
                branch,
                contents
            });

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="branch"></param>
        /// <param name="contents"></param>
        /// <returns></returns>
        public async Task<T> PostAsync<T>(string branch, List<object> contents)
            => await PostAsync<T>(new
            {
                branch,
                contents
            });
    }
}