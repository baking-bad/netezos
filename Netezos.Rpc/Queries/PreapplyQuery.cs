using Netezos.Rpc.Queries.Post;

namespace Netezos.Rpc.Queries
{
    /// <summary>
    /// RPC query to get helpers associated with a preapply objects
    /// </summary>
    public class PreapplyQuery : RpcQuery
    {
        /// <summary>
        /// Gets the query to the block preapply
        /// </summary>
        public PreapplyBlockQuery Block => new PreapplyBlockQuery(this, "block");

        /// <summary>
        /// Gets the query to the operations preapply
        /// </summary>
        public PreapplyOperationQuery Operations => new PreapplyOperationQuery(this, "operations");

        internal PreapplyQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}
