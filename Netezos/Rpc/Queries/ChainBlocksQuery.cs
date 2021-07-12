namespace Netezos.Rpc.Queries
{
    public class ChainBlocksQuery : RpcObject
    {
        internal ChainBlocksQuery(RpcClient client, string query) : base(client, query)
        {
        }
    }
}