using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    public class OperationQuery : RpcPost
    {
        internal OperationQuery(RpcQuery baseQuery, string append) : base(baseQuery, append)
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
            var args = new RpcPostArgs();
            args.Add("branch", branch);
            args.Add("contents", contents);
            return await PostAsync(args);
        }
    }
}