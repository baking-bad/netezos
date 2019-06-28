using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    public class ProtocolDataQuery : RpcPost
    {
        internal ProtocolDataQuery(RpcQuery baseQuery, string append) : base(baseQuery, append)
        {
        }

        /// <summary>
        ///     Forge a protocol data
        /// </summary>
        /// <param name="priority">Priority</param>
        /// <param name="nonceHash">Nonce hash</param>
        /// <param name="powNonce">Proof of work nonce</param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(int priority, string nonceHash, string powNonce)
        {
            return await PostAsync(new
            {
                priority,
                nonce_hash = nonceHash,
                proof_of_work_nonce = powNonce
            });
        }
    }
}