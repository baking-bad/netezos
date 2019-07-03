using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Netezos.Rpc.Queries;

namespace Netezos.Rpc
{
    /// <summary>
    /// Rpc queries builder
    /// </summary>
    public class TezosRpc : IDisposable
    {
        /// <summary>
        /// Gets the query to the blocks
        /// </summary>
        public BlocksQuery Blocks { get; }
        
        /// <summary>
        /// Gets the query to the injection
        /// </summary>
        public InjectionQuery Inject { get; }

        private string Chain { get; }
        private RpcClient Client { get; }

        /// <summary>
        /// Creates the instanse of TezosRpc
        /// </summary>
        /// <param name="uri">Base URI of the node</param>
        /// <param name="chain">Chain to work with</param>
        public TezosRpc(string uri, Chain chain = Rpc.Chain.Main)
        {
            Client = new RpcClient(uri);
            Chain = chain.ToString().ToLower();

            Blocks = new BlocksQuery(Client, $"chains/{Chain}/blocks/");
            Inject = new InjectionQuery(Client, $"injection/");
        }

        /// <summary>
        /// Creates the instanse of TezosRpc
        /// </summary>
        /// <param name="uri">Base URI of the node</param>
        /// <param name="timeout">Timeout in seconds for the requests</param>
        /// <param name="chain">Chain to work with</param>
        public TezosRpc(string uri, int timeout, Chain chain = Rpc.Chain.Main)
        {
            Client = new RpcClient(uri, timeout);
            Chain = chain.ToString().ToLower();

            Blocks = new BlocksQuery(Client, $"chains/{Chain}/blocks/");
            Inject = new InjectionQuery(Client, $"injection/");
        }

        /// <summary>
        /// Sends request and returns the json object
        /// </summary>
        /// <param name="query">Relative path to the RPC method</param>
        /// <returns></returns>
        public async Task<JToken> GetAsync(string query) => await Client.GetJson(query);

        /// <summary>
        /// Sends request and returns the json object, deserialized to the specified type
        /// </summary>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <param name="query">Relative path to the RPC method</param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string query) => await Client.GetJson<T>(query);

        public void Dispose()
        {
            Client?.Dispose();
        }
    }
}
