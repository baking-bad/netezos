using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    public class RevealQuery : RpcPost
    {
        internal RevealQuery(RpcQuery baseQuery) : base(baseQuery)
        {
        }

        /// <summary>
        ///     Forge a delegation operation
        /// </summary>
        /// <param name="branch">Branch</param>
        /// <param name="source">Source</param>
        /// <param name="fee">Operation fee</param>
        /// <param name="storageLimit">Storage limit</param>
        /// <param name="gasLimit">Gas limit</param>
        /// <param name="counter">Counter</param>
        /// <param name="publicKey">Public key</param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(string branch, string source, long fee, long counter, long gasLimit, long storageLimit,
            string publicKey)
        {
            var args = new RpcPostArgs();
            args.Add("branch", branch);
            args.Add("contents", new List<object>
            {
                new
                {
                    kind = "reveal",
                    source,
                    fee = fee.ToString(),
                    counter = counter.ToString(),
                    gas_limit = gasLimit.ToString(),
                    storage_limit = storageLimit.ToString(),
                    public_key = publicKey
                }
            });
            return await PostAsync(args);
        }
    }
}