using System.Collections.Generic;
using System.Threading.Tasks;

namespace Netezos.Rpc.Queries.Post
{
    /// <summary>
    /// Rpc query to access big_map storage
    /// </summary>
    public class BigMapQuery : RpcMethod
    {
        internal BigMapQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Gets the value associated with a key in the big_map storage of the contract
        /// </summary>
        /// <param name="key">Key (micheline michelson expression)</param>
        /// <param name="type">Type of the key (micheline michelson expression)</param>
        /// <param name="prim">Primitive michelson type</param>
        /// <returns></returns>
        public Task<dynamic> PostAsync(object key, string type, string prim)
            => PostAsync(new
            {
                key = new Dictionary<string, object> { { type, key } },
                type = new { prim }
            });

        /// <summary>
        /// Gets the value associated with a key in the big_map storage of the contract
        /// </summary>
        /// <param name="key">Key (micheline michelson expression)</param>
        /// <param name="type">Type of the key (micheline michelson expression)</param>
        public Task<dynamic> PostAsync(object key, object type)
            => PostAsync(new
            {
                key,
                type
            });

        /// <summary>
        /// Gets the value associated with a key in the big_map storage of the contract
        /// </summary>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <param name="key">Key (micheline michelson expression)</param>
        /// <param name="type">Type of the key (micheline michelson expression)</param>
        /// <param name="prim">Primitive michelson type</param>
        /// <returns></returns>
        public Task<T> PostAsync<T>(object key, string type, string prim)
            => PostAsync<T>(new
            {
                key = new Dictionary<string, object> { { type, key } },
                type = new { prim }
            });

        /// <summary>
        /// Gets the value associated with a key in the big_map storage of the contract
        /// </summary>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <param name="key">Key (micheline michelson expression)</param>
        /// <param name="type">Type of the key (micheline michelson expression)</param>
        /// <returns></returns>
        public Task<T> PostAsync<T>(object key, object type)
            => PostAsync<T>(new
            {
                key,
                type
            });
    }
}