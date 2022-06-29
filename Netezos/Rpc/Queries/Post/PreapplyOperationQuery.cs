using Netezos.Forging.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Netezos.Rpc.Queries.Post
{
    /// <summary>
    /// Rpc query to access operations preapplying
    /// </summary>
    public class PreapplyOperationQuery : RpcMethod
    {
        internal PreapplyOperationQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Simulates the validation of an operation and returns the preapplied data
        /// </summary>
        /// <param name="operations">Operations</param>
        /// <returns></returns>
        public Task<dynamic> PostAsync(string protocol, IEnumerable<Operation> operations)
            => PostAsync(operations.Select(op => new {
                protocol,
                signature = op.Signature,
                branch = op.Branch,
                contents = op.Contents
            }));

        /// <summary>
        /// Simulates the validation of an operation and returns the preapplied data
        /// </summary>
        /// <param name="branch">Branch</param>
        /// <param name="protocol">Protocol hash</param>
        /// <param name="signature">Signature</param>
        /// <param name="contents">List of contents</param>
        /// <returns></returns>
        public Task<dynamic> PostAsync(string protocol, string signature, string branch, List<OperationContent> contents)
            => PostAsync(new[]
            {
                new
                {
                    protocol,
                    signature,
                    branch,
                    contents
                }
            });

        /// <summary>
        /// Simulates the validation of an operation and returns the preapplied data
        /// </summary>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <param name="branch">Branch</param>
        /// <param name="protocol">Protocol hash</param>
        /// <param name="signature">Signature</param>
        /// <param name="contents">List of contents</param>
        /// <returns></returns>
        public Task<T> PostAsync<T>(string protocol, string signature, string branch, List<OperationContent> contents)
            => PostAsync<T>(new[]
            {
                new
                {
                    protocol,
                    signature,
                    branch,
                    contents
                }
            });
    }
}