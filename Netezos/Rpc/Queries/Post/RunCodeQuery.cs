using System.Threading.Tasks;

namespace Netezos.Rpc.Queries.Post
{
    /// <summary>
    /// Rpc query to access code running
    /// </summary>
    public class RunCodeQuery : RpcMethod
    {
        internal RunCodeQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Runs a piece of code in the current context and returns the storage, operations and big_map data
        /// </summary>
        /// <param name="script">Script (micheline michelson expression)</param>
        /// <param name="storage">Storage (micheline michelson expression)</param>
        /// <param name="input">Input(micheline michelson expression)</param>
        /// <param name="amount">Amount</param>
        /// <param name="source">Source (optional)</param>
        /// <param name="payer">Payer (optional)</param>
        /// <param name="gas">Gas limit (optional)</param>
        /// <returns></returns>
        public Task<dynamic> PostAsync(object script, object storage, object input, long amount, string source = null, string payer = null, long? gas = null)
            => PostAsync(new
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
        /// Runs a piece of code in the current context and returns the storage, operations and big_map data
        /// </summary>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <param name="script">Script (micheline michelson expression)</param>
        /// <param name="storage">Storage (micheline michelson expression)</param>
        /// <param name="input">Input(micheline michelson expression)</param>
        /// <param name="amount">Amount</param>
        /// <param name="source">Source (optional)</param>
        /// <param name="payer">Payer (optional)</param>
        /// <param name="gas">Gas limit (optional)</param>
        /// <returns></returns>
        public Task<T> PostAsync<T>(object script, object storage, object input, long amount, string source = null, string payer = null, long? gas = null)
            => PostAsync<T>(new
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