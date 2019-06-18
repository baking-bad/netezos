using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    public class ForgeBlockHeaderQuery : RpcPost
    {
        /// <summary>
        /// Forge a protocol data
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
        public async Task<JToken> PostAsync(int level, int proto, string predecessor, DateTime timestamp, int validationPass,
            string operationsHash, List<string> fitness, string context, string protocolData)
        {
            var args = new RpcPostArgs();
            args.Add("level", level);
            args.Add("proto", proto);
            args.Add("predecessor", predecessor);
            args.Add("timestamp", timestamp);
            args.Add("validation_pass", validationPass);
            args.Add("operations_hash", operationsHash);
            args.Add("fitness", fitness);
            args.Add("context", context);
            args.Add("protocol_data", protocolData);
            return await PostAsync(args);
        }
        
        internal ForgeBlockHeaderQuery(RpcQuery baseQuery, string append) : base(baseQuery, append){}
    }
}