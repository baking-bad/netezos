using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    public class TypeCheckDataQuery : RpcPost
    {
        internal TypeCheckDataQuery(RpcQuery baseQuery, string append) : base(baseQuery, append)
        {
        }

        /// <summary>Run code</summary>
        /// <param name="program">Michelson expression</param>
        /// <param name="gas">Gas limit(optional)</param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(object data, object type, long? gas = null)
        {
            return await base.PostAsync(            new
            {
                data,
                type,
                gas = gas?.ToString()
            });
        }
    }
}