using Netezos.Rpc.Queries.Post;

namespace Netezos.Rpc.Queries
{
    /// <summary>
    /// Access the code and data of the contract.
    /// </summary>
    public class ScriptQuery : RpcObject
    {
        /// <summary>
        /// Access the script of the contract and normalize it using the requested unparsing mode.
        /// </summary>
        public NormalizedQuery Normalized => new NormalizedQuery(this, "normalized/");

        internal ScriptQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}