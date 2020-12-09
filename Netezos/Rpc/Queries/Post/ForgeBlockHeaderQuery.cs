using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Netezos.Rpc.Queries.Post
{
    /// <summary>
    /// Rpc query to access block headers forging
    /// </summary>
    public class ForgeBlockHeaderQuery : RpcMethod
    {
        internal ForgeBlockHeaderQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Forges a block header and returns forged bytes
        /// </summary>
        /// <param name="level">Level of the block</param>
        /// <param name="proto">Number of the protocol</param>
        /// <param name="predecessor">Predecessor</param>
        /// <param name="timestamp">Timestamp</param>
        /// <param name="validationPass">Validation pass</param>
        /// <param name="operationsHash">A list of list of operations (Base58Check-encoded)</param>
        /// <param name="fitness">Fitness of the block</param>
        /// <param name="context">Context hash (Base58Check-encoded)</param>
        /// <param name="protocolData">Protocol data hexadecimal string</param>
        /// <returns></returns>
        public Task<dynamic> PostAsync(int level, int proto, string predecessor, DateTime timestamp,
            int validationPass, string operationsHash, List<string> fitness, string context, string protocolData)
                => PostAsync(new
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
        /// Forges a block header and returns forged bytes
        /// </summary>
        /// <param name="level">Level of the block</param>
        /// <param name="proto">Number of the protocol</param>
        /// <param name="predecessor">Predecessor</param>
        /// <param name="timestamp">Timestamp</param>
        /// <param name="validationPass">Validation pass</param>
        /// <param name="operationsHash">A list of list of operations (Base58Check-encoded)</param>
        /// <param name="fitness">Fitness of the block</param>
        /// <param name="context">Context hash (Base58Check-encoded)</param>
        /// <param name="protocolData">Protocol data hexadecimal string</param>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <returns></returns>
        public Task<T> PostAsync<T>(int level, int proto, string predecessor, DateTime timestamp,
            int validationPass, string operationsHash, List<string> fitness, string context, string protocolData)
                => PostAsync<T>(new
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