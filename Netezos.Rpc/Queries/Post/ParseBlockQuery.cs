using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    public class ParseBlockQuery : RpcPost
    {
        internal ParseBlockQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Parse a block. Return a JToken with block header signed contents.
        /// </summary>
        /// <param name="level">Level of block</param>
        /// <param name="proto">Protocol number</param>
        /// <param name="predecessor">Predecessor</param>
        /// <param name="timestamp">Timestamp</param>
        /// <param name="validationPass">Validation pass</param>
        /// <param name="operationsHash">A list of list of operations (Base58Check-encoded)</param>
        /// <param name="fitness">Block fitness</param>
        /// <param name="context">A hash of context (Base58Check-encoded)</param>
        /// <param name="protocolData">Protocol data decimal string</param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(int level, int proto, string predecessor, DateTime timestamp,
            int validationPass, string operationsHash, List<string> fitness, string context, string protocolData)
                => await PostAsync(new
                {
                    level,
                    proto,
                    predecessor,
                    timestamp,
                    validation_pass = validationPass,
                    operations_hash = operationsHash,
                    fitness,
                    context,
                    protocol_data = protocolData
                });

        /// <summary>
        /// Parse a block. Return a JToken with block header signed contents.
        /// </summary>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <param name="level">Level of block</param>
        /// <param name="proto">Protocol number</param>
        /// <param name="predecessor">Predecessor</param>
        /// <param name="timestamp">Timestamp</param>
        /// <param name="validationPass">Validation pass</param>
        /// <param name="operationsHash">A list of list of operations (Base58Check-encoded)</param>
        /// <param name="fitness">Block fitness</param>
        /// <param name="context">A hash of context (Base58Check-encoded)</param>
        /// <param name="protocolData">Protocol data decimal string</param>
        /// <returns></returns>
        public async Task<T> PostAsync<T>(int level, int proto, string predecessor, DateTime timestamp,
            int validationPass, string operationsHash, List<string> fitness, string context, string protocolData)
                => await PostAsync<T>(new
                {
                    level,
                    proto,
                    predecessor,
                    timestamp,
                    validation_pass = validationPass,
                    operations_hash = operationsHash,
                    fitness,
                    context,
                    protocol_data = protocolData
                });
    }
}