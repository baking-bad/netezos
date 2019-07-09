using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    public class TypeCheckCodeQuery : RpcPost
    {
        internal TypeCheckCodeQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Typecheck a piece of code in the current context. Returns the JToken with type map.
        /// </summary>
        /// <param name="program">Program michelson expression</param>
        /// <param name="gas">Gas limit(optional)</param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(object program, long? gas = null)
            => await base.PostAsync(new
            {
                program,
                gas = gas?.ToString()
            });

        /// <summary>
        /// Typecheck a piece of code in the current context. Returns the JToken with type map.
        /// </summary>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <param name="program">Program michelson expression</param>
        /// <param name="gas">Gas limit(optional)</param>
        /// <returns></returns>
        public async Task<T> PostAsync<T>(object program, long? gas = null)
            => await base.PostAsync<T>(new
            {
                program,
                gas = gas?.ToString()
            });
    }
}