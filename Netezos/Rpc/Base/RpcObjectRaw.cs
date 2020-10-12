namespace Netezos.Rpc
{
    /// <summary>
    /// Rpc query to GET a json object
    /// </summary>
    public class RpcObjectRaw : RpcObject
    {
        /// <summary>
        /// Gets the query to the raw (unparsed) data
        /// </summary>
        public RpcObject Raw => new RpcObject(this, "raw/");

        internal RpcObjectRaw(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}
