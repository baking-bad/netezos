using System.Threading.Tasks;

namespace Netezos.Rpc.Queries.Post
{
    /// <summary>
    /// Rpc query to access type checking
    /// </summary>
    public class TypeCheckCodeQuery : RpcMethod
    {
        internal TypeCheckCodeQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Typechecks a piece of code in the current context and returns the type map
        /// </summary>
        /// <param name="program">Program michelson expression</param>
        /// <param name="gas">Gas limit (optional)</param>
        /// <returns></returns>
        public Task<dynamic> PostAsync(object program, long? gas = null)
            => base.PostAsync(new
            {
                program,
                gas = gas?.ToString()
            });

        /// <summary>
        /// Typechecks a piece of code in the current context and returns the type map
        /// </summary>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <param name="program">Program michelson expression</param>
        /// <param name="gas">Gas limit (optional)</param>
        /// <returns></returns>
        public Task<T> PostAsync<T>(object program, long? gas = null)
            => base.PostAsync<T>(new
            {
                program,
                gas = gas?.ToString()
            });
    }
}