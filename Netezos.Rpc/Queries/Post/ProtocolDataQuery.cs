using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    public class ProtocolDataQuery : RpcPost
    {
        /// <summary>
        /// Forge a protocol data
        /// </summary>
        /// <param name="priority">Priority</param>
        /// <param name="nonceHash">Nonce hash</param>
        /// <param name="powNonce">Proof of work nonce</param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(int priority, string nonceHash, string powNonce)
        {
            var args = new RpcPostArgs();
            args.Add("priority", priority);
            args.Add("nonce_hash", nonceHash);
            args.Add("proof_of_work_nonce", powNonce);
            return await PostAsync(args);
        }
        
        internal ProtocolDataQuery(RpcQuery baseQuery, string append) : base(baseQuery, append){}
    }
}