using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    //TODO fix xml docs
    public class ForgeProtocolDataQuery : RpcPost
    {
        internal ForgeProtocolDataQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Forge a protocol data
        /// </summary>
        /// <param name="priority">Priority</param>
        /// <param name="nonceHash">Nonce hash</param>
        /// <param name="powNonce">Proof of work nonce</param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(int priority, string nonceHash, string powNonce)
            => await PostAsync(new
            {
                priority,
                nonce_hash = nonceHash,
                proof_of_work_nonce = powNonce
            });

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="priority"></param>
        /// <param name="nonceHash"></param>
        /// <param name="powNonce"></param>
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