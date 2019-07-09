using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc
{
    /// <summary>
    /// RPC request to Post a json object
    /// </summary>
    public class RpcPost : RpcQuery
    {
        internal RpcPost(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        internal RpcPost(RpcQuery baseQuery) : base(baseQuery) { }

        /// <summary>
        /// Send a POST request with specified string content and returns the json object
        /// </summary>
        /// <param name="content">Json input content</param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(string content) => await Client.PostJson(Query, content);

        /// <summary>
        /// Send a POST request with specified object-type content and returns the json object
        /// </summary>
        /// <param name="content">Data as an object</param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(object content) => await Client.PostJson(Query, content.ToJson());

        /// <summary>
        /// Send a POST request with specified string content and returns the json object, deserialized to the specified type
        /// </summary>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <param name="content">Json input content</param>
        /// <returns></returns>
        public async Task<T> PostAsync<T>(string content) => await Client.PostJson<T>(Query, content);

        /// <summary>
        /// Send a POST request with specified object-type content and returns the json object, deserialized to the specified type
        /// </summary>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <param name="content">Data as an object</param>
        /// <returns></returns>
        public async Task<T> PostAsync<T>(object content) => await Client.PostJson<T>(Query, content.ToJson());

        public override string ToString() => Query;
    }
}
