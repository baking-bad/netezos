﻿using Netezos.Rpc.Queries.Post;

namespace Netezos.Rpc.Queries
{
    /// <summary>
    /// Rpc query to access blocks data
    /// </summary>
    public class InjectionQuery : RpcObject
    {
        /// <summary>
        /// Gets the query to the Block injection
        /// </summary>
        public InjectBlockQuery Block => new InjectBlockQuery(this, "block");
        
        /// <summary>
        /// Gets the query to the operation injection
        /// </summary>
        public InjectOperationQuery Operation => new InjectOperationQuery(this, "operation");
        
        /// <summary>
        /// Gets the query to the protocol injection
        /// </summary>
        public InjectProtocolQuery Protocol => new InjectProtocolQuery(this, "protocol");

        internal InjectionQuery(RpcClient client, string query) : base(client, query) { }
    }
}
