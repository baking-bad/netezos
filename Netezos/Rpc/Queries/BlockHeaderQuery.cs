namespace Netezos.Rpc.Queries
{
    /// <summary>
    /// Rpc query to access block header data
    /// </summary>
    public class BlockHeaderQuery : RpcObjectRaw
    {
        /// <summary>
        /// Gets the query to the protocol data
        /// </summary>
        public RpcObjectRaw ProtocolData => new(this, "protocol_data/");

        /// <summary>
        /// Gets the query to the shell data
        /// </summary>
        public RpcObject Shell => new(this, "shell/");

        internal BlockHeaderQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}
