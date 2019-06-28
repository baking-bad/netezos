using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    public class ForgeOperationsQuery : RpcPost
    {
        internal ForgeOperationsQuery(RpcQuery baseQuery, string append) : base(baseQuery, append)
        {
        }

        /// <summary>
        ///     Forge a Proposals operation
        /// </summary>
        /// <param name="branch">Branch</param>
        /// <param name="contents">List of contents</param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(string branch, List<object> contents)
        {
            return await PostAsync(new
            {
                branch,
                contents
            });
        }
    }
}