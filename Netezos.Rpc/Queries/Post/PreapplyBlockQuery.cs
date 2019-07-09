using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    public class PreapplyBlockQuery : RpcPost
    {
        internal PreapplyBlockQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Simulate the validation of a block that would contain the given operations and return the resulting fitness and context hash.
        /// </summary>
        /// <param name="protocol">Hash of current protocol</param>
        /// <param name="priority">Priority</param>
        /// <param name="powNonce">Proof of work nonce</param>
        /// <param name="signature">Signature</param>
        /// <param name="operations">List of operations</param>
        /// <param name="seedNonceHash">Seed nonce hash(optional)</param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(string protocol, int priority, string powNonce, string signature, List<List<object>> operations, string seedNonceHash = null)
            => await PostAsync(new
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
        /// Simulate the validation of a block with specified timestamp that would contain the given operations and return the resulting fitness and context hash.
        /// </summary>
        /// <param name="protocol">Protocol</param>
        /// <param name="priority">Priority</param>
        /// <param name="powNonce">Proof of work nonce</param>
        /// <param name="signature">Signature</param>
        /// <param name="operations">List of operations</param>
        /// <param name="timestamp">Timestamp</param>
        /// <param name="sort">Sort(optional)</param>
        /// <param name="seedNonceHash">Seed nonce hash(optional)</param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(string protocol, int priority, string powNonce, string signature,
            List<List<object>> operations, DateTime timestamp, bool sort = false, string seedNonceHash = null)
                => await Client.PostJson(
                    $"{Query}?sort={sort}&timestamp={timestamp.ToEpoch()}",
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
                    }.ToJson());

        /// <summary>
        /// Simulate the validation of a block that would contain the given operations and return the resulting fitness and context hash.
        /// </summary>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <param name="protocol">Hash of current protocol</param>
        /// <param name="priority">Priority</param>
        /// <param name="powNonce">Proof of work nonce</param>
        /// <param name="signature">Signature</param>
        /// <param name="operations">List of operations</param>
        /// <param name="seedNonceHash">Seed nonce hash(optional)</param>
        /// <returns></returns>
        public async Task<T> PostAsync<T>(string protocol, int priority, string powNonce, string signature, List<List<object>> operations, string seedNonceHash = null)
            => await PostAsync<T>(new
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
        /// Simulate the validation of a block with specified timestamp that would contain the given operations and return the resulting fitness and context hash.
        /// </summary>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <param name="protocol">Protocol</param>
        /// <param name="priority">Priority</param>
        /// <param name="powNonce">Proof of work nonce</param>
        /// <param name="signature">Signature</param>
        /// <param name="operations">List of operations</param>
        /// <param name="timestamp">Timestamp</param>
        /// <param name="sort">Sort(optional)</param>
        /// <param name="seedNonceHash">Seed nonce hash(optional)</param>
        /// <returns></returns>
        public async Task<T> PostAsync<T>(string protocol, int priority, string powNonce, string signature,
            List<List<object>> operations, DateTime timestamp, bool sort = false, string seedNonceHash = null)
                => await Client.PostJson<T>(
                    $"{Query}?sort={sort}&timestamp={timestamp.ToEpoch()}",
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
                    }.ToJson());
    }
}