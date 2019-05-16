namespace Netezos.Rpc.Queries
{
    /// <summary>
    /// Rpc query to access constants data
    /// </summary>
    public class ConstantsQuery : RpcObject
    {
        /// <summary>
        /// Gets the query to the schema for all RPC errors from this protocol version
        /// </summary>
        public RpcObject Errors => new RpcObject(this, "errors/");

        internal ConstantsQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}
