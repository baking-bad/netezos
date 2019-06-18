using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    public class SeedNonceRevelationQuery : RpcPost
    {
        /// <summary>
        /// Forge a seed nonce revelation operation
        /// </summary>
        /// <param name="branch">Branch</param>
        /// <param name="level">Level</param>
        /// <param name="nonce">Nonce</param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(string branch, int level, string nonce)
        {
            var args = new RpcPostArgs();
            args.Add("branch", branch);
            args.Add("contents", new List<object>
            {
                new 
                {
                    kind = "seed_nonce_revelation",
                    level,
                    nonce
                }
            });
            return await PostAsync(args);
        }
        
        internal SeedNonceRevelationQuery(RpcQuery baseQuery) : base(baseQuery) {}
    }
}