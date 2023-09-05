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
        public NormalizedQuery Normalized => new(this, "normalized/");
        
        /// <summary>
        /// Access the paid storage space of the contract.
        /// </summary>
        public RpcObject PaidSpace => new(this, "paid_space/");
        
        /// <summary>
        /// Access the used storage space of the contract.
        /// </summary>
        public RpcObject UsedSpace => new(this, "used_space/");

        internal StorageQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}