using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    /// <summary>
    /// Rpc query to access the seed
    /// </summary>
    public class SeedQuery : RpcPost
    {
        internal SeedQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Gets the seed of the cycle to which the block belongs
        /// </summary>
        /// <returns></returns>
        public Task<JToken> PostAsync() => PostAsync(new { });

        /// <summary>
        /// Gets the seed of the cycle to which the block belongs
        /// </summary>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <returns></returns>
        public Task<T> PostAsync<T>() => PostAsync<T>(new { });
    }
}