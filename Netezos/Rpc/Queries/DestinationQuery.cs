namespace Netezos.Rpc.Queries
{
    /// <summary>
    /// Rpc query to access destination data
    /// </summary>
    public class DestinationQuery : RpcObject
    {
        /// <summary>
        /// Returns the index assigned to the address if it was indexed by the opcode INDEX_ADDRESS, otherwise returns null
        /// </summary>
        public RpcObject Index => new(this, "index/");

        internal DestinationQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }

    /// <summary>
    /// Rpc query to access destination data by destination id
    /// </summary>
    public class DestinationsQuery : RpcObject
    {
        /// <summary>
        /// Gets the query to the destination data by destination id
        /// </summary>
        /// <param name="destinationId">A destination identifier encoded in b58check</param>
        public DestinationQuery this[string destinationId] => new(this, $"{destinationId}/");

        internal DestinationsQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}