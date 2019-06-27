using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    public class PreapplyDoubleEndorsementEvidenceQuery : RpcPost
    {
        internal PreapplyDoubleEndorsementEvidenceQuery(RpcQuery baseQuery) : base(baseQuery)
        {
        }

        /// <summary>
        ///     Forge a double endorsement evidence operation
        /// </summary>
        /// <param name="branch">Branch</param>
        /// <param name="endorsement1">First inlined endorsement object</param>
        /// <param name="endorsement2">Second inlined endorsement object</param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(string branch, object endorsement1, object endorsement2)
        {
            var args = new RpcPostArgs();
            args.Add("branch", branch);
            args.Add("contents", new List<object>
            {
                new
                {
                    kind = "double_endorsement_evidence",
                    op1 = endorsement1,
                    op2 = endorsement2
                }
            });
            return await PostListAsync(args);
        }
    }
}