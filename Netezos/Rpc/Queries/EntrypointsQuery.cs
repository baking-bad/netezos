using Netezos.Rpc.Queries.Post;

namespace Netezos.Rpc.Queries
{
    public class EntrypointsQuery : RpcObject
    {

        /// <summary>
        /// Return the type of the given entrypoint of the contract
        /// </summary>
        public RpcObject this[string entrypoint] => new RpcObject(this, $"{entrypoint}/");


        internal EntrypointsQuery(RpcQuery baseQuery, string append) : base(baseQuery, append)
        {
        }
    }
}