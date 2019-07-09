using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    public class BigMapQuery : RpcPost
    {
        internal BigMapQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Access the value associated with a key in the big map storage of the contract. Returns JToken with a big map.
        /// </summary>
        /// <param name="key">Key. Micheline michelson expression.</param>
        /// <param name="type">Type of Key. Micheline michelson expression.</param>
        /// <param name="prim">Primitive michelson type</param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(object key, string type, string prim)
            => await PostAsync(new
            {
                key = new Dictionary<string, object> { { type, key } },
                type = new { prim }
            });

        /// <summary>
        /// Access the value associated with a key in the big map storage of the contract. Returns JToken with a big map.
        /// </summary>
        /// <param name="key">Key. Micheline michelson expression.</param>
        /// <param name="type">Type of Key. Micheline michelson expression.</param>
        public async Task<JToken> PostAsync(object key, object type)
            => await PostAsync(new
            {
                key,
                type
            });

        /// <summary>
        /// Access the value associated with a key in the big map storage of the contract. Returns JToken with a big map.
        /// </summary>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <param name="key">Key. Micheline michelson expression.</param>
        /// <param name="type">Type of Key. Micheline michelson expression.</param>
        /// <param name="prim">Primitive michelson type</param>
        /// <returns></returns>
        public async Task<T> PostAsync<T>(object key, string type, string prim)
            => await PostAsync<T>(new
            {
                key = new Dictionary<string, object> { { type, key } },
                type = new { prim }
            });

        /// <summary>
        /// Access the value associated with a key in the big map storage of the contract. Returns JToken with a big map.
        /// </summary>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <param name="key">Key. Micheline michelson expression.</param>
        /// <param name="type">Type of Key. Micheline michelson expression.</param>
        /// <returns></returns>
        public async Task<T> PostAsync<T>(object key, object type)
            => await PostAsync<T>(new
            {
                key,
                type
            });
    }
}