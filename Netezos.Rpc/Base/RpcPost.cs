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
        /// Send a POST request with specified json content and returns the json object
        /// </summary>
        /// <param name="content">Json content to send</param>
        /// <returns></returns>
        public virtual Task<JToken> PostAsync(string content) => Client.PostJson(Query, content);

        /// <summary>
        /// Send a POST request with specified json object content and returns the json object
        /// </summary>
        /// <param name="content">Object to send</param>
        /// <returns></returns>
        public virtual Task<JToken> PostAsync(object content) => Client.PostJson(Query, content.ToJson());

        /// <summary>
        /// Send a POST request with specified json content and returns the json object, deserialized to the specified type
        /// </summary>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <param name="content">Json content to send</param>
        /// <returns></returns>
        public virtual Task<T> PostAsync<T>(string content) => Client.PostJson<T>(Query, content);

        /// <summary>
        /// Send a POST request with specified json object content and returns the json object, deserialized to the specified type
        /// </summary>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <param name="content">Object to send</param>
        /// <returns></returns>
        public virtual Task<T> PostAsync<T>(object content) => Client.PostJson<T>(Query, content.ToJson());

        public override string ToString() => Query;
    }
}
