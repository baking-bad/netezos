using System;
using System.Net.Http;
using System.Threading.Tasks;
using Netezos.Rpc.Queries;

namespace Netezos.Rpc
{
    /// <summary>
    /// Rpc queries builder
    /// </summary>
    public class TezosRpc : IDisposable
    {
        /// <summary>
        /// The chain unique identifier.
        /// </summary>
        public RpcObject ChainId => new RpcObject(ChainQuery, "chain_id/");

        /// <summary>
        /// The current checkpoint for this chain.
        /// </summary>
        public RpcObject Checkpoint => new RpcObject(ChainQuery, "checkpoint/");

        /// <summary>
        /// Lists blocks that have been declared invalid along with the errors that led to them being declared invalid.
        /// </summary>
        public RpcDictionary<string, RpcObject> InvalidBlocks
            => new RpcDictionary<string, RpcObject>(ChainQuery, "invalid_blocks/");

        /// <summary>
        /// The bootstrap status of a chain
        /// </summary>
        public RpcObject IsBootstrapped => new RpcObject(ChainQuery, "is_bootstrapped/");

        /// <summary>
        /// Gets the query to the mempool data associated with the block
        /// </summary>
        public MempoolQuery Mempool => new MempoolQuery(ChainQuery, "mempool/");

        /// <summary>
        /// Gets the query to the blocks
        /// </summary>
        public BlocksQuery Blocks => new BlocksQuery(ChainQuery, "blocks/");

        /// <summary>
        /// Gets the query to the injection
        /// </summary>
        public InjectionQuery Inject => new InjectionQuery(Client, $"injection/");

        /// <summary>
        /// Gets the query to the config 
        /// </summary>
        public ConfigQuery Config => new ConfigQuery(Client, "config/network/");

        string Chain { get; }
        RpcClient Client { get; }
        RpcQuery ChainQuery { get; }

        /// <summary>
        /// Creates the instanse of TezosRpc
        /// </summary>
        /// <param name="uri">Base URI of the node</param>
        /// <param name="chain">Chain to work with.
        /// Note: this is not a network (mainnet or testnet), but an ID of the chain, in case you use a multi-chain node.</param>
        public TezosRpc(string uri, string chain = "main")
        {
            Chain = chain;
            Client = new RpcClient(uri);
            ChainQuery = new RpcQuery(Client, $"chains/{Chain}/");
        }

        /// <summary>
        /// Creates the instanse of TezosRpc
        /// </summary>
        /// <param name="uri">Base URI of the node</param>
        /// <param name="timeout">Timeout in seconds for the requests</param>
        /// <param name="chain">Chain to work with.
        /// Note: this is not a network (mainnet or testnet), but an ID of the chain, in case you use a multi-chain node.</param>
        public TezosRpc(string uri, int timeout, string chain = "main")
        {
            Chain = chain;
            Client = new RpcClient(uri, timeout);
            ChainQuery = new RpcQuery(Client, $"chains/{Chain}/");
        }

        /// <summary>
        /// Creates the instanse of TezosRpc
        /// </summary>
        /// <param name="client">HttpClient instanse that will be used for sending RPC requests.</param>
        /// <param name="chain">Chain to work with.
        /// Note: this is not a network (mainnet or testnet), but an ID of the chain, in case you use a multi-chain node.</param>
        public TezosRpc(HttpClient client, string chain = "main")
        {
            Chain = chain;
            Client = new RpcClient(client);
            ChainQuery = new RpcQuery(Client, $"chains/{Chain}/");
        }

        /// <summary>
        /// Sends request and returns the dynamic json object
        /// </summary>
        /// <param name="query">Relative path to the RPC method</param>
        /// <returns></returns>
        public Task<dynamic> GetAsync(string query) => Client.GetJson(query);

        /// <summary>
        /// Sends request and returns the json object, deserialized to the specified type
        /// </summary>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <param name="query">Relative path to the RPC method</param>
        /// <returns></returns>
        public Task<T> GetAsync<T>(string query) => Client.GetJson<T>(query);

        /// <summary>
        /// Releases the unmanaged resourses and disposes of the managed resources used by the <c>TezosRpc</c>
        /// </summary>
        public void Dispose() => Client?.Dispose();
    }
}
