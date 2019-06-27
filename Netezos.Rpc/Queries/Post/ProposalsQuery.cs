using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    public class ProposalsQuery : RpcPost
    {
        internal ProposalsQuery(RpcQuery baseQuery) : base(baseQuery)
        {
        }

        /// <summary>
        ///     Forge a Proposals operation
        /// </summary>
        /// <param name="branch">Branch</param>
        /// <param name="source">Tz Address</param>
        /// <param name="period">Period</param>
        /// <param name="proposals">List of proposal hashes</param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(string branch, string source, int period, List<string> proposals)
        {
            var args = new RpcPostArgs();
            args.Add("branch", branch);
            args.Add("contents", new List<object>
            {
                new
                {
                    kind = "proposals",
                    source,
                    period,
                    proposals
                }
            });
            return await PostAsync(args);
        }
    }
}