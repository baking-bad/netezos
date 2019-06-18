using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    public class SeedQuery : RpcPost
    {
        /// <summary>
        /// Seed of the cycle to which the block belongs.
        /// </summary>
        /// <returns></returns>
        public async Task<JToken> PostAsync()
        {
            return await PostAsync(new RpcPostArgs());
        }
        
        internal SeedQuery(RpcQuery baseQuery, string append) : base(baseQuery, append){}
    }
}