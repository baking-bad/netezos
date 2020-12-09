using Netezos.Rpc.Queries.Post;

namespace Netezos.Rpc.Queries
{
    /// <summary>
    /// RPC query to get preapplying helpers associated with a block
    /// </summary>
    public class PreapplyQuery : RpcQuery
    {
        /// <summary>
        /// Gets the query to the block preapplying
        /// </summary>
        public PreapplyBlockQuery Block => new PreapplyBlockQuery(this, "block");

        /// <summary>
        /// Gets the query to the operations preapplying
        /// </summary>
        public PreapplyOperationQuery Operations => new PreapplyOperationQuery(this, "operations");

        internal PreapplyQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}
