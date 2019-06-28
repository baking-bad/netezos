using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    public class RunOperationQuery : RpcPost
    {
        internal RunOperationQuery(RpcQuery baseQuery, string append) : base(baseQuery, append)
        {
        }

        /// <summary>Run code</summary>
        /// <param name="branch">Branch</param>
        /// <param name="contents">List of contents</param>
        /// <param name="signature">Signature</param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(string branch, List<object> contents, string signature)
        {
            return await PostAsync(new
            {
                branch,
                contents,
                signature
            });
        }
    }
}