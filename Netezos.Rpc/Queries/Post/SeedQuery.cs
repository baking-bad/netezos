using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    public class SeedQuery : RpcPost
    {
        internal SeedQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Seed of the cycle to which the block belongs. Returns cycle seed bytes.
        /// </summary>
        /// <returns></returns>
        public async Task<JToken> PostAsync() => await PostAsync(new { });

        /// <summary>
        /// Seed of the cycle to which the block belongs. Returns cycle seed bytes.
        /// </summary>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <returns></returns>
        public async Task<T> PostAsync<T>() => await PostAsync<T>(new { });
    }
}