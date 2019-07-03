using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    //TODO fix xml docs
    public class SeedQuery : RpcPost
    {
        internal SeedQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Seed of the cycle to which the block belongs.
        /// </summary>
        /// <returns></returns>
        public async Task<JToken> PostAsync() => await PostAsync(new { });

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<T> PostAsync<T>() => await PostAsync<T>(new { });
    }
}