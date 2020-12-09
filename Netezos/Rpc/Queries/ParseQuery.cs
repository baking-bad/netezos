using Netezos.Rpc.Queries.Post;

namespace Netezos.Rpc.Queries
{
    /// <summary>
    /// RPC query to get data parsing helpers associated with a block
    /// </summary>
    public class ParseQuery : RpcQuery
    {
        /// <summary>
        /// Gets the query to the block parsing
        /// </summary>
        public ParseBlockQuery Block => new ParseBlockQuery(this, "block/");

        /// <summary>
        /// Gets the query to the operations parsing
        /// </summary>
        public ParseOperationsQuery Operations => new ParseOperationsQuery(this, "operations/");

        internal ParseQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}
