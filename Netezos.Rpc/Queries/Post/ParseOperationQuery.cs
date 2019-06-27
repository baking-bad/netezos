using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    public class ParseOperationQuery : RpcPost
    {
        internal ParseOperationQuery(RpcQuery baseQuery, string append) : base(baseQuery, append)
        {
        }

        /// <summary>
        ///     Forge a protocol data
        /// </summary>
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
        public async Task<JToken> PostAsync(List<object> operations, bool? checkSignature = null)
        {
            var args = new RpcPostArgs();
            args.Add("operations", operations);
            if (checkSignature != null)
                args.Add("check_signature", checkSignature);
            return await PostAsync(args);
        }
    }
}