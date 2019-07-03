using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    //TODO fix xml docs
    public class RunCodeQuery : RpcPost
    {
        internal RunCodeQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Run code
        /// </summary>
        /// <param name="script">Script</param>
        /// <param name="storage">Storage</param>
        /// <param name="input">Input</param>
        /// <param name="amount">Amount</param>
        /// <param name="source">Source(optional)</param>
        /// <param name="payer">Payer(optional)</param>
        /// <param name="gas">Gas limit(optional)</param>
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
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="script"></param>
        /// <param name="storage"></param>
        /// <param name="input"></param>
        /// <param name="amount"></param>
        /// <param name="source"></param>
        /// <param name="payer"></param>
        /// <param name="gas"></param>
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