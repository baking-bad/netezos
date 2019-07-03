using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    //TODO fix xml docs
    public class ParseOperationQuery : RpcPost
    {
        internal ParseOperationQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Forge a protocol data
        /// </summary>
        /// <param name="operations"></param>
        /// <param name="checkSignature"></param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(List<object> operations, bool? checkSignature = null)
            => await PostAsync(new
            {
                operations,
                check_signature = checkSignature
            });

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="operations"></param>
        /// <param name="checkSignature"></param>
        /// <returns></returns>
        public async Task<T> PostAsync<T>(List<object> operations, bool? checkSignature = null)
            => await PostAsync<T>(new
            {
                operations,
                check_signature = checkSignature
            });
    }
}