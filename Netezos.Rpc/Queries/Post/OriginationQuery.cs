using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    public class OriginationQuery : RpcPost
    {
        internal OriginationQuery(RpcQuery baseQuery) : base(baseQuery)
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
        /// <param name="delegatePubKey">Delegate public key</param>
        /// <param name="ManagerPubKey">Manager public key</param>
        /// <param name="balance">Balance</param>
        /// <param name="spendable">Spendable(optional)</param>
        /// <param name="delegatable">Delegatable(optional)</param>
        /// <param name="script">Script(optional)</param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(string branch, string source, long fee, long counter, long gasLimit, long storageLimit,
            string ManagerPubKey, long balance, bool? spendable = null, bool? delegatable = null, string delegatePubKey = null,
            string script = null)
        {
            var args = new RpcPostArgs();
            args.Add("branch", branch);
            args.Add("contents", new List<object>
            {
                new
                {
                    kind = "origination",
                    source,
                    fee = fee.ToString(),
                    counter = counter.ToString(),
                    gas_limit = gasLimit.ToString(),
                    storage_limit = storageLimit.ToString(),
                    manager_pubkey = ManagerPubKey,
                    balance = balance.ToString(),
                    spendable,
                    delegatable,
                    @delegate = delegatePubKey,
                    script
                }
            });
            return await PostAsync(args);
        }
    }
}