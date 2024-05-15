namespace Netezos.Rpc.Queries.Dal
{
    public class DalQuery : RpcQuery
    {
        /// <summary>
        /// Gets the query to the (currently last) DAL skip list cell if DAL is enabled, or [None] otherwise.
        /// </summary>
        public RpcObject CommitmentsHistory => new(this, "commitments_history/");

        /// <summary>
        /// Gets the query to the published slots headers for the given level.
        /// </summary>
        public PublishedSlotHeadersQuery PublishedSlotHeaders => new(this, "published_slot_headers/");

        /// <summary>
        /// Gets the query to the shards assignment for a given level (the default is the current level) and given
        /// delegates (the default is all delegates)
        /// </summary>
        public ShardsQuery Shards => new(this, "shards/");

        internal DalQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}
