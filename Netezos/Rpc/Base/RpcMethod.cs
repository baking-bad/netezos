using System.Threading.Tasks;

namespace Netezos.Rpc
{
    /// <summary>
    /// RPC query to POST a json object
    /// </summary>
    public class RpcMethod : RpcQuery
    {
        internal RpcMethod(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Send a POST request with specified json content and returns the dynamic json object
        /// </summary>
        /// <param name="content">Json content to send</param>
        /// <returns></returns>
        public Task<dynamic> PostAsync(string content) => Client.PostJson(Query, content);

        /// <summary>
        /// Send a POST request with specified json object content and returns the dynamic json object
        /// </summary>
        /// <param name="content">Object to send</param>
        /// <returns></returns>
        public Task<dynamic> PostAsync(object content) => Client.PostJson(Query, content);

        /// <summary>
        /// Send a POST request with specified json content and returns the json object, deserialized to the specified type
        /// </summary>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <param name="content">Json content to send</param>
        /// <returns></returns>
        public Task<T> PostAsync<T>(string content) => Client.PostJson<T>(Query, content);

        /// <summary>
        /// Send a POST request with specified json object content and returns the json object, deserialized to the specified type
        /// </summary>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <param name="content">Object to send</param>
        /// <returns></returns>
        public Task<T> PostAsync<T>(object content) => Client.PostJson<T>(Query, content);
    }
}
