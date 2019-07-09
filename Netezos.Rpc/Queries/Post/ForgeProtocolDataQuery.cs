using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    /// <summary>
    /// Rpc query to access protocol data forging
    /// </summary>
    public class ForgeProtocolDataQuery : RpcPost
    {
        internal ForgeProtocolDataQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Forges the protocol-specific part of a block header and returns forged bytes
        /// </summary>
        /// <param name="priority">Priority</param>
        /// <param name="nonceHash">Nonce hash</param>
        /// <param name="powNonce">Proof of work nonce (optional)</param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(int priority, string nonceHash, string powNonce)
            => await PostAsync(new
            {
                priority,
                nonce_hash = nonceHash,
                proof_of_work_nonce = powNonce
            });

        /// <summary>
        /// Forges the protocol-specific part of a block header and returns forged bytes
        /// </summary>
        /// <param name="priority">Priority</param>
        /// <param name="nonceHash">Nonce hash</param>
        /// <param name="powNonce">Proof of work nonce (optional)</param>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <returns></returns>
        public async Task<T> PostAsync<T>(int priority, string nonceHash, string powNonce)
            => await PostAsync<T>(new
            {
                priority,
                nonce_hash = nonceHash,
                proof_of_work_nonce = powNonce
            });
    }
}