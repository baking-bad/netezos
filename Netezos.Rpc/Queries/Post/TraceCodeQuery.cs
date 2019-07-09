using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    /// <summary>
    /// Rpc query to access code trace
    /// </summary>
    public class TraceCodeQuery : RpcPost
    {
        internal TraceCodeQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Runs a piece of code in the current context, keeping a trace, and returns the storage, operations, trace and big_map data
        /// </summary>
        /// <param name="script">Script (micheline michelson expression)</param>
        /// <param name="storage">Storage (micheline michelson expression)</param>
        /// <param name="input">Input (micheline michelson expression)</param>
        /// <param name="amount">Amount</param>
        /// <param name="source">Source (optional)</param>
        /// <param name="payer">Payer (optional)</param>
        /// <param name="gas">Gas limit (optional)</param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(object script, object storage, object input, long amount, string source = null, string payer = null, long? gas = null)
            => await PostAsync(new
            {
                script,
                storage,
                input,
                amount = amount.ToString(),
                source,
                payer,
                gas = gas?.ToString()
            });

        /// <summary>
        /// Runs a piece of code in the current context, keeping a trace, and returns the storage, operations, trace and big_map data
        /// </summary>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <param name="script">Script (micheline michelson expression)</param>
        /// <param name="storage">Storage (micheline michelson expression)</param>
        /// <param name="input">Input (micheline michelson expression)</param>
        /// <param name="amount">Amount</param>
        /// <param name="source">Source (optional)</param>
        /// <param name="payer">Payer (optional)</param>
        /// <param name="gas">Gas limit (optional)</param>
        /// <returns></returns>
        public async Task<T> PostAsync<T>(object script, object storage, object input, long amount, string source = null, string payer = null, long? gas = null)
            => await PostAsync<T>(new
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