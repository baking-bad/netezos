using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    public class PackDataQuery : RpcPost
    {
        internal PackDataQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Computes the serialized version of some data expression using the same algorithm as script instruction PACK
        /// </summary>
        /// <param name="data">Micheline michelson expression</param>
        /// <param name="type">Type of data. Micheline michelson expression</param>
        /// <param name="gas">Gas limit</param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(object data, object type, long? gas = null)
            => await PostAsync(new
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
        /// <param name="type">Type of data. Micheline michelson expression</param>
        /// <param name="gas">Gas limit</param>
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