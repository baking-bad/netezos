using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc
{
    /// <summary>
    /// Rpc query to get a json object
    /// </summary>
    public class DeepRpcObject : RpcObject
    {
        internal DeepRpcObject(RpcClient client, string query) : base(client, query) { }
        internal DeepRpcObject(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Executes the query and returns the json object
        /// </summary>
        /// <param name="depth">Depth</param>
        /// <returns></returns>
        public Task<JToken> GetAsync(int depth) => Client.GetJson($"{Query}?depth={depth}");

        /// <summary>
        /// Executes the query and returns the json object, deserealized to the specified type
        /// </summary>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <param name="depth">Depth</param>
        /// <returns></returns>
        public Task<T> GetAsync<T>(int depth) => Client.GetJson<T>($"{Query}?depth={depth}");

        public override string ToString() => Query;
    }
}
