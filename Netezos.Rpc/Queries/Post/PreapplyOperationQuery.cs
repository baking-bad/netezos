using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    public class PreapplyOperationQuery : RpcPost
    {
        internal PreapplyOperationQuery(RpcQuery baseQuery, string append) : base(baseQuery, append)
        {
        }

        /// <summary>
        ///     Forge a double baking evidence operation
        /// </summary>
        /// <param name="branch">Branch</param>
        /// <param name="protocol">Protocol</param>
        /// <param name="signature">Signature</param>
        /// <param name="contents">List of contents</param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(string protocol, string signature, string branch, List<object> contents)
        {
            return await PostListAsync(new
            {
                protocol,
                signature,
                branch,
                contents
            });
        }
    }
}