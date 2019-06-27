using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    public class BallotQuery : RpcPost
    {
        internal BallotQuery(RpcQuery baseQuery) : base(baseQuery)
        {
        }

        /// <summary>
        ///     Forge a ballot operation
        /// </summary>
        /// <param name="branch">Branch</param>
        /// <param name="source">Tz Address</param>
        /// <param name="period">Period</param>
        /// <param name="proposalHash">Proposal hash</param>
        /// <param name="ballot">"nay" | "yay" | "pass"</param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(string branch, string source, int period, string proposalHash, string ballot)
        {
            var args = new RpcPostArgs();
            args.Add("branch", branch);
            args.Add("contents", new List<object>
            {
                new
                {
                    kind = "ballot",
                    source,
                    period,
                    proposal = proposalHash,
                    ballot = ballot.ToLower()
                }
            });
            return await PostAsync(args);
        }
    }
}