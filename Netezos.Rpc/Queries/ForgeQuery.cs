using Netezos.Rpc.Queries.Post;

namespace Netezos.Rpc.Queries
{
    public class ForgeQuery : RpcQuery
    {
        
        /// <summary>
        /// Gets the query to the operations
        /// </summary>
        public ForgeOperationsQuery Operations => new ForgeOperationsQuery(this, "operations/");
        /// <summary>
        /// Gets the query to the protocol data
        /// </summary>
        public ProtocolDataQuery ProtocolData => new ProtocolDataQuery(this, "protocol_data/");

        internal ForgeQuery(RpcQuery baseQuery, string append) : base(baseQuery, append){}
    }
}