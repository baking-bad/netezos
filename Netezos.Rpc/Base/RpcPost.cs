using System;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc
{
    //TODO Edit summary
    /// <summary>
    /// Rpc query to Post a json object
    /// </summary>
    public class RpcPost : RpcQuery
    {
        internal RpcPost(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        internal RpcPost(RpcQuery baseQuery) : base(baseQuery) {}

        /// <summary>
        /// Executes the query and returns the json object
        /// </summary>
        /// <param name="args">Json input args</param>
        /// <returns></returns>
        protected async Task<JToken> PostAsync(RpcPostArgs args) => await Client.Post(Query, args.ToString());

        /// <summary>
        /// Executes the query and returns the json object, deserialized to the specified type
        /// </summary>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <returns></returns>
        protected async Task<T> PostAsync<T>(RpcPostArgs args) => await Client.Post<T>(Query, args.ToString());

        public override string ToString() => Query;
    }
}
