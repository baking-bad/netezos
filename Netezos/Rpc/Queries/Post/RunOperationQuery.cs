using System.Collections.Generic;
using System.Threading.Tasks;

namespace Netezos.Rpc.Queries.Post
{
    /// <summary>
    /// Rpc query to access operations running
    /// </summary>
    public class RunOperationQuery : RpcMethod
    {
        internal RunOperationQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Runs an operation without signature checks and returns the operation result
        /// </summary>
        /// <param name="branch">Branch</param>
        /// <param name="contents">List of operation contents</param>
        /// <param name="signature">Signature</param>
        /// <returns></returns>
        public Task<dynamic> PostAsync(string branch, List<object> contents, string signature)
            => PostAsync(new
            {
                branch,
                contents,
                signature
            });

        /// <summary>
        /// Runs an operation without signature checks and returns the operation result
        /// </summary>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <param name="branch">Branch</param>
        /// <param name="contents">List of operation contents</param>
        /// <param name="signature">Signature</param>
        /// <returns></returns>
        public Task<T> PostAsync<T>(string branch, List<object> contents, string signature)
            => PostAsync<T>(new
            {
                branch,
                contents,
                signature
            });
    }
}