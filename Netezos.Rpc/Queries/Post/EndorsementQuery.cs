using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    public class EndorsementQuery : RpcPost
    {
        /// <summary>
        /// Forge an endorsement operation
        /// </summary>
        /// <param name="branch">Branch</param>
        /// <param name="level">Block level</param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(string branch, int level)
        {
            var args = new RpcPostArgs();
            args.Add("branch", branch);
            args.Add("contents", new List<object>
            {
                new 
                {
                    kind = "endorsement",
                    level
                }
            });
            return await PostAsync(args);
        }
        
        internal EndorsementQuery(RpcQuery baseQuery) : base(baseQuery) {}
    }
}