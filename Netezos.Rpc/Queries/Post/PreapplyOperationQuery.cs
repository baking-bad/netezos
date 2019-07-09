using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    /// <summary>
    /// Rpc query to access operations preapplying
    /// </summary>
    public class PreapplyOperationQuery : RpcPost
    {
        internal PreapplyOperationQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Simulates the validation of an operation and returns the preapplied data
        /// </summary>
        /// <param name="branch">Branch</param>
        /// <param name="protocol">Protocol hash</param>
        /// <param name="signature">Signature</param>
        /// <param name="contents">List of contents</param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(string protocol, string signature, string branch, List<object> contents)
            => await PostAsync(new[]
            {
                new
                {
                    protocol,
                    signature,
                    branch,
                    contents
                }
            });

        /// <summary>
        /// Simulates the validation of an operation and returns the preapplied data
        /// </summary>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <param name="branch">Branch</param>
        /// <param name="protocol">Protocol hash</param>
        /// <param name="signature">Signature</param>
        /// <param name="contents">List of contents</param>
        /// <returns></returns>
        public async Task<T> PostAsync<T>(string protocol, string signature, string branch, List<object> contents)
            => await PostAsync<T>(new[]
            {
                new
                {
                    protocol,
                    signature,
                    branch,
                    contents
                }
            });
    }
}