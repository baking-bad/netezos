using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    public class PreapplyOperationQuery : RpcPost
    {
        internal PreapplyOperationQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Simulate the validation of an operation. Returns JToken with data about preapplied operation.
        /// </summary>
        /// <param name="branch">Hash of the current head</param>
        /// <param name="protocol">Current protocol hash</param>
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
        /// Simulate the validation of an operation. Returns JToken with data about preapplied operation.
        /// </summary>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <param name="branch">Hash of the current head</param>
        /// <param name="protocol">Current protocol hash</param>
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