using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    //TODO fix xml docs
    //TODO check preapplying of an array
    public class PreapplyOperationQuery : RpcPost
    {
        internal PreapplyOperationQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Forge a double baking evidence operation
        /// </summary>
        /// <param name="branch">Branch</param>
        /// <param name="protocol">Protocol</param>
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
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="protocol"></param>
        /// <param name="signature"></param>
        /// <param name="branch"></param>
        /// <param name="contents"></param>
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