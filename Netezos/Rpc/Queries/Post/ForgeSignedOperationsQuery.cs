namespace Netezos.Rpc.Queries.Post
{
    /// <summary>
    /// Rpc query to forge a signed operation
    /// </summary>
    public class ForgeSignedOperationsQuery : RpcMethod
    {
        internal ForgeSignedOperationsQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Forges a signed operation and returns operation bytes
        /// </summary>
        /// <param name="branch">Block hash</param>
        /// <param name="contents">List of operation contents</param>
        /// <param name="signature">Optional signature</param>
        public Task<dynamic> PostAsync(string branch, List<object> contents, string? signature = null)
            => PostAsync(new
            {
                branch,
                contents,
                signature
            });

        /// <summary>
        /// Forges a signed operation and returns operation bytes
        /// </summary>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <param name="branch">Block hash</param>
        /// <param name="contents">List of operation contents</param>
        /// <param name="signature">Optional signature</param>
        public Task<T?> PostAsync<T>(string branch, List<object> contents, string? signature = null)
            => PostAsync<T>(new
            {
                branch,
                contents,
                signature
            });
    }
}
