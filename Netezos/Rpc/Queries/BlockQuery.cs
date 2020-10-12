namespace Netezos.Rpc.Queries
{
    /// <summary>
    /// Rpc query to access block data
    /// </summary>
    public class BlockQuery : RpcObject
    {
        /// <summary>
        /// Gets the query to the block hash
        /// </summary>
        public RpcObject Hash => new RpcObject(this, "hash/");

        /// <summary>
        /// Gets the query to the block header
        /// </summary>
        public BlockHeaderQuery Header => new BlockHeaderQuery(this, "header/");

        /// <summary>
        /// Gets the query to the block metadata
        /// </summary>
        public RpcObject Metadata => new RpcObject(this, "metadata/");

        /// <summary>
        /// Gets the query to the list of the ancestors of the block which,
        /// if referred to as the branch in an operation header, are recent enough
        /// for that operation to be included in the current block
        /// </summary>
        public RpcObject LiveBlocks => new RpcObject(this, "live_blocks/");

        /// <summary>
        /// Gets the query to the context associated with the block
        /// </summary>
        public ContextQuery Context => new ContextQuery(this, "context/");

        /// <summary>
        /// Gets the query to the helpers associated with the block
        /// </summary>
        public HelpersQuery Helpers => new HelpersQuery(this, "helpers/");

        /// <summary>
        /// Gets the query to votes data associated with the block
        /// </summary>
        public VotesQuery Votes => new VotesQuery(this, "votes/");

        /// <summary>
        /// Gets the query to the list of lists of operations hashes in the block
        /// </summary>
        public RpcArray<RpcArray<RpcObject>> OperationsHashes
            => new RpcArray<RpcArray<RpcObject>>(this, "operation_hashes/");

        /// <summary>
        /// Gets the query to the list of lists of operations in the block
        /// </summary>
        public RpcArray<RpcArray<RpcObject>> Operations
            => new RpcArray<RpcArray<RpcObject>>(this, "operations/");

        internal BlockQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}
