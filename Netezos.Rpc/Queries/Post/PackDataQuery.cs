using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    public class PackDataQuery : RpcPost
    {
        internal PackDataQuery(RpcQuery baseQuery, string append) : base(baseQuery, append)
        {
        }

        /// <summary>
        ///     Access the value associated with a key in the big map storage of the contract.
        /// </summary>
        /// <param name="data">Data</param>
        /// <param name="type">Type of Key</param>
        /// <param name="gas">Gas limit</param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(object data, object type, long? gas = null)
        {
            return await PostAsync(            new
            {
                data,
                type,
                gas = gas?.ToString()
            });
        }
    }
}