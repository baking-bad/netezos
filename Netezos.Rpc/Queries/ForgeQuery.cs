using Netezos.Rpc.Queries.Post;

namespace Netezos.Rpc.Queries
{
    //TODO fix xml docs
    public class ForgeQuery : RpcQuery
    {
        /// <summary>
        /// Gets the query to the forging
        /// </summary>
        public ForgeBlockHeaderQuery BlockHeader => new ForgeBlockHeaderQuery(Base, "forge_block_header/");

        /// <summary>
        /// Gets the query to the protocol data
        /// </summary>
        public ForgeProtocolDataQuery ProtocolData => new ForgeProtocolDataQuery(this, "protocol_data/");

        /// <summary>
        /// Gets the query to the operations
        /// </summary>
        public ForgeOperationsQuery Operations => new ForgeOperationsQuery(this, "operations/");

        internal ForgeQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}