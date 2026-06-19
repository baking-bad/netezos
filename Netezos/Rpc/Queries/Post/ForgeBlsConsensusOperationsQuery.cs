namespace Netezos.Rpc.Queries.Post
{
    /// <summary>
    /// Rpc query to forge a consensus operation in BLS mode
    /// </summary>
    public class ForgeBlsConsensusOperationsQuery : RpcMethod
    {
        internal ForgeBlsConsensusOperationsQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Forges a consensus operation in BLS mode
        /// </summary>
        /// <param name="branch">Block hash</param>
        /// <param name="contents">List of operation contents</param>
        public Task<dynamic> PostAsync(string branch, List<object> contents)
            => PostAsync(new
            {
                branch,
                contents
            });

        /// <summary>
        /// Forges a consensus operation in BLS mode
        /// </summary>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <param name="branch">Block hash</param>
        /// <param name="contents">List of operation contents</param>
        public Task<T?> PostAsync<T>(string branch, List<object> contents)
            => PostAsync<T>(new
            {
                branch,
                contents
            });
    }
}
