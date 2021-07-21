using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Netezos.Forging.Models;

namespace Netezos.Rpc.Queries.Post
{
    /// <summary>
    /// Rpc query to access blocks preapplying
    /// </summary>
    public class PreapplyBlockQuery : RpcMethod
    {
        internal PreapplyBlockQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Simulates the validation of a block that would contain the given operations and returns the resulting fitness and context hash
        /// </summary>
        /// <param name="protocol">Protocol hash</param>
        /// <param name="priority">Priority</param>
        /// <param name="powNonce">Proof of work nonce</param>
        /// <param name="signature">Signature</param>
        /// <param name="operations">List of operations</param>
        /// <param name="seedNonceHash">Seed nonce hash (optional)</param>
        /// <returns></returns>
        public Task<dynamic> PostAsync(string protocol, int priority, string powNonce, string signature, IEnumerable<IEnumerable<object>> operations, string seedNonceHash = null)
            => PostAsync(new
            {
                protocol_data = new
                {
                    protocol,
                    priority,
                    proof_of_work_nonce = powNonce,
                    seed_nonce_hash = seedNonceHash,
                    signature
                },
                operations
            });

        /// <summary>
        /// Simulates the validation of a block that would contain the given operations and returns the resulting fitness and context hash
        /// </summary>
        /// <param name="protocol">Protocol hash</param>
        /// <param name="priority">Priority</param>
        /// <param name="powNonce">Proof of work nonce</param>
        /// <param name="signature">Signature</param>
        /// <param name="operations">List of operations</param>
        /// <param name="timestamp">Timestamp</param>
        /// <param name="sort">Sort (optional)</param>
        /// <param name="seedNonceHash">Seed nonce hash (optional)</param>
        /// <returns></returns>
        public Task<dynamic> PostAsync(string protocol, int priority, string powNonce, string signature,
            IEnumerable<IEnumerable<object>> operations, DateTime timestamp, bool sort = false, string seedNonceHash = null)
                => Client.PostJson(
                    $"{Query}?sort={sort}&timestamp={timestamp.ToUnixTime()}",
                    new
                    {
                        protocol_data = new
                        {
                            protocol,
                            priority,
                            proof_of_work_nonce = powNonce,
                            seed_nonce_hash = seedNonceHash,
                            signature
                        },
                        operations
                    });

        /// <summary>
        /// Simulates the validation of a block that would contain the given operations and returns the resulting fitness and context hash
        /// </summary>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <param name="protocol">Protocol hash</param>
        /// <param name="priority">Priority</param>
        /// <param name="powNonce">Proof of work nonce</param>
        /// <param name="signature">Signature</param>
        /// <param name="operations">List of operations</param>
        /// <param name="seedNonceHash">Seed nonce hash (optional)</param>
        /// <returns></returns>
        public Task<T> PostAsync<T>(string protocol, int priority, string powNonce, string signature, IEnumerable<IEnumerable<object>> operations, string seedNonceHash = null)
            => PostAsync<T>(new
            {
                protocol_data = new
                {
                    protocol,
                    priority,
                    proof_of_work_nonce = powNonce,
                    seed_nonce_hash = seedNonceHash,
                    signature
                },
                operations
            });

        /// <summary>
        /// Simulates the validation of a block that would contain the given operations and returns the resulting fitness and context hash
        /// </summary>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <param name="protocol">Protocol hash</param>
        /// <param name="priority">Priority</param>
        /// <param name="powNonce">Proof of work nonce</param>
        /// <param name="signature">Signature</param>
        /// <param name="operations">List of operations</param>
        /// <param name="timestamp">Timestamp</param>
        /// <param name="sort">Sort (optional)</param>
        /// <param name="seedNonceHash">Seed nonce hash (optional)</param>
        /// <returns></returns>
        public Task<T> PostAsync<T>(string protocol, int priority, string powNonce, string signature,
            IEnumerable<IEnumerable<object>> operations, DateTime timestamp, bool sort = false, string seedNonceHash = null)
                => Client.PostJson<T>(
                    $"{Query}?sort={sort}&timestamp={timestamp.ToUnixTime()}",
                    new
                    {
                        protocol_data = new
                        {
                            protocol,
                            priority,
                            proof_of_work_nonce = powNonce,
                            seed_nonce_hash = seedNonceHash,
                            signature
                        },
                        operations
                    });

        /// <summary>
        /// Simulates the activation protocol of a block that would contain the given operations and returns the resulting shell header
        /// </summary>
        /// <param name="protocol">Protocol hash</param>
        /// <param name="command">Priority</param>
        /// <param name="hash">Signature</param>
        /// <param name="fitness">Fitness</param>
        /// <param name="protocolParameters"></param>
        /// <param name="signature">Signature</param>
        /// <param name="operations">List of operations</param>
        /// <param name="timestamp">Timestamp</param>
        /// <param name="sort"></param>
        /// <returns></returns>
        internal Task<dynamic> PostAsync(string protocol, string command, string hash, IEnumerable<string> fitness,
            string protocolParameters, string signature, IEnumerable<IEnumerable<object>> operations, DateTime timestamp, bool sort = false)
            => Client.PostJson(
                $"{Query}?sort={sort}&timestamp={timestamp.ToUnixTime()}",
                new
                {
                    protocol_data = new
                    {
                        protocol,
                        content = new
                        {
                          command,
                          hash,
                          fitness,
                          protocol_parameters = protocolParameters
                        },
                        signature
                    },
                    operations
                });

        /// <summary>
        /// Simulates the activation protocol of a block that would contain the given operations and returns the resulting shell header
        /// </summary>
        /// <param name="protocol">Protocol hash</param>
        /// <param name="command">Priority</param>
        /// <param name="hash">Signature</param>
        /// <param name="fitness">Fitness</param>
        /// <param name="protocolParameters"></param>
        /// <param name="signature">Signature</param>
        /// <param name="operations">List of operations</param>
        /// <param name="timestamp">Timestamp</param>
        /// <param name="sort"></param>
        /// <returns></returns>
        internal Task<T> PostAsync<T>(string protocol, string command, string hash, IEnumerable<string> fitness,
            string protocolParameters, string signature, IEnumerable<IEnumerable<object>> operations, DateTime timestamp, bool sort = false)
            => Client.PostJson<T>(
                $"{Query}?sort={sort}&timestamp={timestamp.ToUnixTime()}",
                new
                {
                    protocol_data = new
                    {
                        protocol,
                        content = new
                        {
                            command,
                            hash,
                            fitness,
                            protocol_parameters = protocolParameters
                        },
                        signature
                    },
                    operations
                });
    }
}