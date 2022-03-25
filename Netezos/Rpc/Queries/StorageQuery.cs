using Netezos.Rpc.Queries.Post;

namespace Netezos.Rpc.Queries
{
    /// <summary>
    /// Access the data of the contract.
    /// </summary>
    public class StorageQuery : RpcObject
    {
        /// <summary>
        /// Access the data of the contract and normalize it using the requested unparsing mode.
        /// </summary>
        public NormalizedQuery Normalized => new NormalizedQuery(this, "normalized/");

        internal StorageQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}