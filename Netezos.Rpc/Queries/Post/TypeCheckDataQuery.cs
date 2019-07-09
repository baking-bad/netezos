using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    public class TypeCheckDataQuery : RpcPost
    {
        internal TypeCheckDataQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Check that some data expression is well formed and of a given type in the current context. Returns the JToken with consumed gas.
        /// </summary>
        /// <param name="data">Data expression</param>
        /// <param name="type">Data expression type</param>
        /// <param name="gas">Gas limit(optional)</param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(object data, object type, long? gas = null)
            => await PostAsync(new
            {
                data,
                type,
                gas = gas?.ToString()
            });

        /// <summary>
        /// Check that some data expression is well formed and of a given type in the current context. Returns the JToken with consumed gas.
        /// </summary>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <param name="data">Data expression</param>
        /// <param name="type">Data expression type</param>
        /// <param name="gas">Gas limit(optional)</param>
        /// <returns></returns>
        public async Task<T> PostAsync<T>(object data, object type, long? gas = null)
            => await PostAsync<T>(new
            {
                data,
                type,
                gas = gas?.ToString()
            });
    }
}