using System.Threading.Tasks;

namespace Netezos.Rpc.Queries.Post
{
    /// <summary>
    /// Rpc query to access type checking
    /// </summary>
    public class TypeCheckDataQuery : RpcMethod
    {
        internal TypeCheckDataQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Checks that some data expression is well formed and of a given type in the current context and returns the consumed gas
        /// </summary>
        /// <param name="data">Data expression</param>
        /// <param name="type">Data expression type</param>
        /// <param name="gas">Gas limit (optional)</param>
        /// <returns></returns>
        public Task<dynamic> PostAsync(object data, object type, long? gas = null)
            => PostAsync(new
            {
                data,
                type,
                gas = gas?.ToString()
            });

        /// <summary>
        /// Checks that some data expression is well formed and of a given type in the current context and returns the consumed gas
        /// </summary>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <param name="data">Data expression</param>
        /// <param name="type">Data expression type</param>
        /// <param name="gas">Gas limit (optional)</param>
        /// <returns></returns>
        public Task<T> PostAsync<T>(object data, object type, long? gas = null)
            => PostAsync<T>(new
            {
                data,
                type,
                gas = gas?.ToString()
            });
    }
}