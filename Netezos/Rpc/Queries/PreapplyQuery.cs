using Netezos.Rpc.Queries.Post;

namespace Netezos.Rpc.Queries
{
    /// <summary>
    /// RPC query to get pre-applying helpers associated with a block
    /// </summary>
    public class PreapplyQuery : RpcQuery
    {
        /// <summary>
        /// Gets the query to the block pre-applying
        /// </summary>
        public PreapplyBlockQuery Block => new(this, "block");

        /// <summary>
        /// Gets the query to the operations pre-applying
        /// </summary>
        public PreapplyOperationQuery Operations => new(this, "operations");

        internal PreapplyQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}
