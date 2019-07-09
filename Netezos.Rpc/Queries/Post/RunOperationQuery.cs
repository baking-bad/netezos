using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    public class RunOperationQuery : RpcPost
    {
        internal RunOperationQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Run an operation without signature checks. Returns the operation result.
        /// </summary>
        /// <param name="branch">Block hash</param>
        /// <param name="contents">List of operation contents</param>
        /// <param name="signature">Signature</param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(string branch, List<object> contents, string signature)
            => await PostAsync(new
            {
                branch,
                contents,
                signature
            });

        /// <summary>
        /// Run an operation without signature checks. Returns the operation result.
        /// </summary>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <param name="branch">Block hash</param>
        /// <param name="contents">List of operation contents</param>
        /// <param name="signature">Signature</param>
        /// <returns></returns>
        public async Task<T> PostAsync<T>(string branch, List<object> contents, string signature)
            => await PostAsync<T>(new
            {
                branch,
                contents,
                signature
            });
    }
}