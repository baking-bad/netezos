using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    public class PreapplyEndorsementQuery : RpcPost
    {
        internal PreapplyEndorsementQuery(RpcQuery baseQuery) : base(baseQuery)
        {
        }

        /// <summary>
        ///     Preapply an endorsement operation
        /// </summary>
        /// <param name="protocol">Protocol</param>
        /// <param name="branch">Branch</param>
        /// <param name="level">Block level</param>
        /// <param name="signature">Signature</param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(string protocol, string branch, int level, string signature)
        {
            var args = new RpcPostArgs();
            args.Add("protocol", protocol);
            args.Add("branch", branch);
            args.Add("contents", new List<object>
            {
                new
                {
                    kind = "endorsement",
                    level
                }
            });
            args.Add("signature", signature);
            return await PostListAsync(args);
        }
    }
}