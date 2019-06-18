using Netezos.Rpc.Queries.Post;

namespace Netezos.Rpc.Queries
{
    /// <summary>
    /// Rpc query to get helpers assotiated with a block
    /// </summary>
    public class ParseQuery : RpcQuery
    {
        /// <summary>
        /// Gets the query to the baking rights
        /// </summary>
        public ParseBlockQuery Block => new ParseBlockQuery(this, "block/");

        /// <summary>
        /// Gets the query to the endorsing rights
        /// </summary>
        public ParseOperationQuery Operations => new ParseOperationQuery(this, "operations/");


        internal ParseQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}
