using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    public class ForgeProtocolDataQuery : RpcPost
    {
        internal ForgeProtocolDataQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Forge the protocol-specific part of a block header. Returns the JToken with protocol data bytes.
        /// </summary>
        /// <param name="priority">Priority</param>
        /// <param name="nonceHash">Nonce hash</param>
        /// <param name="powNonce">Proof of work nonce(optional)</param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(int priority, string nonceHash, string powNonce)
            => await PostAsync(new
            {
                priority,
                nonce_hash = nonceHash,
                proof_of_work_nonce = powNonce
            });

        /// <summary>
        /// Forge the protocol-specific part of a block header. Returns the JToken with protocol data bytes.
        /// </summary>
        /// <param name="priority">Priority</param>
        /// <param name="nonceHash">Nonce hash</param>
        /// <param name="powNonce">Proof of work nonce(optional)</param>
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