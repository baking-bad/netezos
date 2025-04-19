namespace Netezos.Rpc.Queries.Dal
{
    public class DalQuery : RpcQuery
    {
        /// <summary>
        /// Gets the query to the (currently last) DAL skip list cell if DAL is enabled, or [None] otherwise.
        /// </summary>
        public RpcObject CommitmentsHistory => new(this, "commitments_history/");
        
        /// <summary>
        /// Returns the cells of the DAL skip list constructed during this targeted block and stored in the context. The
        /// cells ordering in the list is not specified (not relevant). The list is expected to be empty if the entry is
        /// not initialized in the context (yet), or to have a size that coincides with the number of DAL slots
        /// otherwise.
        /// </summary>
        public RpcObject SkipListCellsOfLevel => new(this, "skip_list_cells_of_level/");

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
