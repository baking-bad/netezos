using Netezos.Rpc.Queries.Post;

namespace Netezos.Rpc.Queries
{
    public class BigMapsQuery : RpcObject
    {

        /// <summary>
        /// Access the value associated with a key in a big map, normalize the output using the requested unparsing mode.
        /// </summary>
        public NormalizedQuery Normalized => new NormalizedQuery(this, "normalized/");

        internal BigMapsQuery(RpcQuery baseQuery, string append) : base(baseQuery, append)
        {
        }
    }
}