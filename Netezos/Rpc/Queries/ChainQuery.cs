namespace Netezos.Rpc.Queries
{
    /// <summary>
    /// Rpc query to access data of current chain
    /// </summary>
    public class ChainQuery : RpcObject
    {
        /// <summary>
        /// The chain unique identifier.
        /// </summary>
        public RpcObject ChainId => new RpcObject(this, "chain_id/");

        /// <summary>
        /// Lists block hashes from '', up to the last checkpoint, sorted with decreasing fitness.
        /// Without arguments it returns the head of the chain. Optional arguments allow to return the list of predecessors of a given block or of a set of blocks.
        /// </summary>
        public RpcObject Blocks => new RpcObject(this, "blocks/");

        /// <summary>
        /// The current checkpoint for this chain.
        /// </summary>
        public RpcObject Checkpoint => new RpcObject(this, "checkpoint/");

        /// <summary>
        /// Lists blocks that have been declared invalid along with the errors that led to them being declared invalid.
        /// </summary>
        public RpcObject InvalidBlocks => new RpcObject(this, "invalid_blocks/");

        /// <summary>
        /// The bootstrap status of a chain
        /// </summary>
        public RpcObject IsBootstrapped => new RpcObject(this, "is_bootstrapped/");

        /// <summary>
        /// Gets the query to the mempool data associated with the block
        /// </summary>
        public MempoolQuery Mempool => new MempoolQuery(this, "mempool/");

        internal ChainQuery(RpcClient client, string query) : base(client, query) { }

    }
}