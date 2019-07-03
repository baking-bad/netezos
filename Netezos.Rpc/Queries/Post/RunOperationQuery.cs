using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    //TODO fix xml docs
    public class RunOperationQuery : RpcPost
    {
        internal RunOperationQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Run code
        /// </summary>
        /// <param name="branch">Branch</param>
        /// <param name="contents">List of contents</param>
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
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="branch"></param>
        /// <param name="contents"></param>
        /// <param name="signature"></param>
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