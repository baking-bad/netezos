using System.Threading.Tasks;

namespace Netezos.Rpc.Queries.Post
{
    /// <summary>
    /// Rpc query to access data packing
    /// </summary>
    public class PackDataQuery : RpcMethod
    {
        internal PackDataQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Computes the serialized version of some data expression using the same algorithm as script instruction PACK
        /// </summary>
        /// <param name="data">Micheline michelson expression</param>
        /// <param name="type">Type of the data (micheline michelson expression)</param>
        /// <param name="gas">Gas limit</param>
        /// <returns></returns>
        public Task<dynamic> PostAsync(object data, object type, long? gas = null)
            => PostAsync(new
            {
                data,
                type,
                gas = gas?.ToString()
            });

        /// <summary>
        /// Computes the serialized version of some data expression using the same algorithm as script instruction PACK
        /// </summary>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <param name="data">Micheline michelson expression</param>
        /// <param name="type">Type of the data (micheline michelson expression)</param>
        /// <param name="gas">Gas limit</param>
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