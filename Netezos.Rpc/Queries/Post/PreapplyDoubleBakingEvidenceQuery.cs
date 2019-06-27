using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    public class PreapplyDoubleBakingEvidenceQuery : RpcPost
    {
        internal PreapplyDoubleBakingEvidenceQuery(RpcQuery baseQuery) : base(baseQuery)
        {
        }

        /// <summary>
        ///     Forge a double baking evidence operation
        /// </summary>
        /// <param name="branch">Branch</param>
        /// <param name="blockHeader1">First block header</param>
        /// <param name="blockHeader2">Second block header</param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(string protocol, string signature, string branch, object blockHeader1, object blockHeader2)
        {
            var args = new RpcPostArgs();
            args.Add("protocol", protocol);
            args.Add("signature", signature);
            args.Add("branch", branch);
            args.Add("contents", new List<object>
            {
                new
                {
                    kind = "double_baking_evidence",
                    bh1 = blockHeader1,
                    bh2 = blockHeader2
                }
            });
            return await PostListAsync(args);
        }
    }
}