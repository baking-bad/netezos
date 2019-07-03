using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    //TODO fix xml docs
    public class BigMapQuery : RpcPost
    {
        internal BigMapQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Access the value associated with a key in the big map storage of the contract.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="type">Type of Key</param>
        /// <param name="prim">Primitive type</param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(object key, string type, string prim)
            => await PostAsync(new
            {
                key = new Dictionary<string, object> { { type, key } },
                type = new { prim }
            });

        /// <summary>
        /// Access the value associated with a key in the big map storage of the contract.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="type">Type of Key</param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(object key, object type)
            => await PostAsync(new
            {
                key,
                type
            });

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <param name="prim"></param>
        /// <returns></returns>
        public async Task<T> PostAsync<T>(object key, string type, string prim)
            => await PostAsync<T>(new
            {
                key = new Dictionary<string, object> { { type, key } },
                type = new { prim }
            });

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<T> PostAsync<T>(object key, object type)
            => await PostAsync<T>(new
            {
                key,
                type
            });
    }
}