namespace Netezos.Rpc.Queries
{
    /// <summary>
    /// Rpc query to access nonces data
    /// </summary>
    public class NoncesQuery : RpcQuery
    {
        /// <summary>
        /// Gets the query to the nonce of a previous block
        /// </summary>
        /// <param name="level">Level of the block</param>
        /// <returns></returns>
        public RpcObject this[int level] => new RpcObject(this, $"{level}/");

        internal NoncesQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}
