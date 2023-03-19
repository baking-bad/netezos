namespace Netezos.Rpc.Queries
{
    /// <summary>
    /// Rpc query to access the contract entrypoints
    /// </summary>
    public class EntrypointsQuery : RpcObject
    {
        /// <summary>
        /// Return the type of the given entrypoint of the contract
        /// </summary>
        /// <param name="entrypoint">Entrypoint of the contract</param>
        /// <returns></returns>
        public RpcObject this[string entrypoint] => new(this, $"{entrypoint}/");

        internal EntrypointsQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}