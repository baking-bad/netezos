using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    public class RunCodeQuery : RpcPost
    {
        internal RunCodeQuery(RpcQuery baseQuery, string append) : base(baseQuery, append)
        {
        }

        /// <summary>Run code</summary>
        /// <param name="script">Script</param>
        /// <param name="storage">Storage</param>
        /// <param name="input">Input</param>
        /// <param name="amount">Amount</param>
        /// <param name="source">Source(optional)</param>
        /// <param name="payer">Payer(optional)</param>
        /// <param name="gas">Gas limit(optional)</param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(object script, object storage, object input, long amount, string source = null, string payer = null, long? gas = null)
        {
            return await PostAsync(            new
            {
                script,
                storage,
                input,
                amount = amount.ToString(),
                source,
                payer,
                gas = gas?.ToString()
            });
        }
    }
}