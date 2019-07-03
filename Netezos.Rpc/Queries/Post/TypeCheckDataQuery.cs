using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    //TODO fix xml docs
    public class TypeCheckDataQuery : RpcPost
    {
        internal TypeCheckDataQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Run code
        /// </summary>
        /// <param name="gas">Gas limit(optional)</param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(object data, object type, long? gas = null)
            => await PostAsync(new
            {
                data,
                type,
                gas = gas?.ToString()
            });

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="type"></param>
        /// <param name="gas"></param>
        /// <returns></returns>
        public async Task<T> PostAsync<T>(object data, object type, long? gas = null)
            => await PostAsync<T>(new
            {
                data,
                type,
                gas = gas?.ToString()
            });
    }
}