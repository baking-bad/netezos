using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    public class PreapplyBlockQuery : RpcPost
    {
        internal PreapplyBlockQuery(RpcQuery baseQuery, string append) : base(baseQuery, append)
        {
        }

        /// <summary>
        ///     Forge a protocol data
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="priority"></param>
        /// <param name="powNonce"></param>
        /// <param name="signature"></param>
        /// <param name="operations"></param>
        /// <param name="seedNonceHash"></param>
        /// <param name="level">Level</param>
        /// <param name="proto">Proto</param>
        /// <param name="predecessor">Predecessor</param>
        /// <param name="timestamp">Timestamp</param>
        /// <param name="validationPass">Validation pass</param>
        /// <param name="operationsHash">A list of list of operations (Base58Check-encoded)</param>
        /// <param name="fitness">Block fitness</param>
        /// <param name="context">A hash of context (Base58Check-encoded)</param>
        /// <param name="protocolData">Protocol data decimal string</param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(string protocol, int priority, string powNonce, string signature, List<List<object>> operations,
            string seedNonceHash = null)
        {
            var args = new RpcPostArgs();
            args.Add("protocol_data", new
            {
                protocol,
                priority,
                proof_of_work_nonce = powNonce,
                seed_nonce_hash = seedNonceHash,
                signature
            });
            args.Add("operations", operations);
            return await PostAsync(args);
        }
    }
}