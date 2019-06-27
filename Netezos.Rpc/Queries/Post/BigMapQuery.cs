using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    public class BigMapQuery : RpcPost
    {
        internal BigMapQuery(RpcQuery baseQuery, string append) : base(baseQuery, append)
        {
        }

        /// <summary>
        ///     Access the value associated with a key in the big map storage of the contract.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="type">Type of Key</param>
        /// <param name="prim">Primitive type</param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(object key, string type, string prim)
        {
            var args = new RpcPostArgs();
            args.Add(key, type, prim);
            return await PostAsync(args);
        }
    }
}