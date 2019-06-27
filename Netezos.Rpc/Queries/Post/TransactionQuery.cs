using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    public class TransactionQuery : RpcPost
    {
        internal TransactionQuery(RpcQuery baseQuery) : base(baseQuery)
        {
        }

        /// <summary>
        ///     Forge a Proposals operation
        /// </summary>
        /// <param name="branch">Branch</param>
        /// <param name="source">Source</param>
        /// <param name="destination">Destination</param>
        /// <param name="amount">Amount</param>
        /// <param name="fee">Transaction fee</param>
        /// <param name="storageLimit">Storage limit</param>
        /// <param name="gasLimit">Gas limit</param>
        /// <param name="counter">Counter</param>
        /// <param name="parameters">Parameters(optional)</param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(string branch, string source, string destination, long amount, long fee,
            long storageLimit, long gasLimit, long counter, RpcPostArgs parameters = null)
        {
            var args = new RpcPostArgs();
            args.Add("branch", branch);
            args.Add("contents", new List<object>
            {
                new
                {
                    kind = "transaction",
                    source,
                    destination,
                    amount = amount.ToString(),
                    fee = fee.ToString(),
                    storage_limit = storageLimit.ToString(),
                    gas_limit = gasLimit.ToString(),
                    counter = counter.ToString(),
                    parameters
                }
            });
            return await PostAsync(args);
        }
    }
}