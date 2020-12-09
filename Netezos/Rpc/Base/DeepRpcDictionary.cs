using System.Threading.Tasks;

namespace Netezos.Rpc
{
    /// <summary>
    /// Rpc query to get a dictionary of json objects, which can also be accessed by key
    /// </summary>
    /// <typeparam name="TKey">Type of the keys to access the objects in the dictionary</typeparam>
    /// <typeparam name="TValue">Type of the objects in the dictionary</typeparam>
    public class DeepRpcDictionary<TKey, TValue> : RpcDictionary<TKey, TValue> where TValue : RpcObject
    {
        internal DeepRpcDictionary(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Executes the query and returns the dynamic json object
        /// </summary>
        /// <param name="depth">Depth</param>
        /// <returns></returns>
        public Task<dynamic> GetAsync(int depth) => Client.GetJson($"{Query}?depth={depth}");

        /// <summary>
        /// Executes the query and returns the json object, deserealized to the specified type
        /// </summary>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <param name="depth">Depth</param>
        /// <returns></returns>
        public Task<T> GetAsync<T>(int depth) => Client.GetJson<T>($"{Query}?depth={depth}");
    }
}
