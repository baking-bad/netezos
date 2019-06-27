using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    public class ActivateAccountQuery : RpcPost
    {
        internal ActivateAccountQuery(RpcQuery baseQuery) : base(baseQuery)
        {
        }

        /// <summary>
        ///     Forge an account activation operation
        /// </summary>
        /// <param name="branch">Branch</param>
        /// <param name="pubKeyHash">Public key hash</param>
        /// <param name="secret">Secret</param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(string branch, string pubKeyHash, string secret)
        {
            var args = new RpcPostArgs();
            args.Add("branch", branch);
            args.Add("contents", new List<object>
            {
                new
                {
                    kind = "activate_account",
                    pkh = pubKeyHash,
                    secret
                }
            });
            return await PostAsync(args);
        }
    }
}