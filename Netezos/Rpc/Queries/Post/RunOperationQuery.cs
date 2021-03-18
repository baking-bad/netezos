using System.Threading.Tasks;
using Netezos.Forging.Models;

namespace Netezos.Rpc.Queries.Post
{
    /// <summary>
    /// Rpc query to access operations running
    /// </summary>
    public class RunOperationQuery : RpcMethod
    {
        const string ChainStub = "NetXdQprcVkpaWU";
        const string BranchStub = "BLockGenesisGenesisGenesisGenesisGenesisf79b5d1CoW2";
        const string SignatureStub = "sigQFenAPMsrMxVvgH1K33sJgj5VqD3gajK1sBJyEugCzZ9EgTvHGiXii9opAgkei7tY1qpJKyB37YGdGGMAWdgodPyDcQvg";

        internal RunOperationQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Runs an operation without signature checks and returns the operation result
        /// </summary>
        /// <param name="contents">Operation content</param>
        /// <returns></returns>
        public Task<dynamic> PostAsync(params OperationContent[] contents)
            => PostAsync(new
            {
                operation = new
                {
                    branch = BranchStub,
                    contents,
                    signature = SignatureStub
                },
                chain_id = ChainStub
            });

        /// <summary>
        /// Runs an operation without signature checks and returns the operation result
        /// </summary>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <param name="contents">Operation content</param>
        /// <returns></returns>
        public Task<T> PostAsync<T>(params OperationContent[] contents)
            => PostAsync<T>(new
            {
                operation = new
                {
                    branch = BranchStub,
                    contents,
                    signature = SignatureStub
                },
                chain_id = ChainStub
            });

        /// <summary>
        /// Runs an operation without signature checks and returns the operation result
        /// </summary>
        /// <param name="chain_id">Chain id</param>
        /// <param name="branch">Branch</param>
        /// <param name="contents">List of operation contents</param>
        /// <returns></returns>
        public Task<dynamic> PostAsync(string chain_id, string branch, params OperationContent[] contents)
            => PostAsync(new
            {
                operation = new
                {
                    branch,
                    contents,
                    signature = SignatureStub
                },
                chain_id
            });

        /// <summary>
        /// Runs an operation without signature checks and returns the operation result
        /// </summary>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <param name="chain_id">Chain id</param>
        /// <param name="branch">Branch</param>
        /// <param name="contents">List of operation contents</param>
        /// <returns></returns>
        public Task<T> PostAsync<T>(string chain_id, string branch, params OperationContent[] contents)
            => PostAsync<T>(new
            {
                operation = new
                {
                    branch,
                    contents,
                    signature = SignatureStub
                },
                chain_id
            });
    }
}