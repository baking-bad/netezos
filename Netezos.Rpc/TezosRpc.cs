using System;
using System.Net.Http;
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
        public static readonly TimeSpan DEFAULT_REQUEST_TIMEOUT = TimeSpan.FromSeconds(10);
        public static readonly Chain DEFAULT_CHAIN = Rpc.Chain.Main;

        /// <summary>
        /// Gets the query to the blocks
        /// </summary>
        public BlocksQuery Blocks { get; }

        /// <summary>
        /// Gets the query to the injection
        /// </summary>
        public InjectionQuery Inject { get; }

        private RpcClient Client { get; }

        [Obsolete("Use typed TezosRpc(Uri) instead.")]
        public TezosRpc(string baseUri)
            : this(new Uri(baseUri))
        {
        }

        [Obsolete("Use typed TezosRpc(Uri, Chain) instead.")]
        public TezosRpc(string baseUri, Chain chain = Rpc.Chain.Main)
            : this(new Uri(baseUri), chain)
        {
        }

        [Obsolete("Use typed TezosRpc(Uri, TimeSpan, Chain) instead.")]
        public TezosRpc(string baseUri, int requestTimeout, Chain chain = Rpc.Chain.Main)
            : this(new Uri(baseUri), TimeSpan.FromSeconds(requestTimeout), chain)
        {
        }

        /// <summary>
        /// Create a TezosRpc instance using a URI endpoint.
        /// </summary>
        /// <param name="baseUri">Base URI of node</param>
        public TezosRpc(Uri baseUri)
            : this(baseUri, DEFAULT_CHAIN)
        {
        }

        /// <summary>
        /// Create a TezosRpc instance using a URI endpoint and a Chain.
        /// </summary>
        /// <param name="baseUri">Base URI of node</param>
        /// <param name="chain">Chain to work with</param>
        public TezosRpc(Uri baseUri, Chain chain)
            : this(baseUri, DEFAULT_REQUEST_TIMEOUT, chain)
        {
        }

        /// <summary>
        /// Create a TezosRpc instance using a URI endpoint, a Chain, and a request timeout.
        /// </summary>
        /// <param name="baseUri">Base URI of node</param>
        /// <param name="requestTimeout">Request timeout</param>
        /// <param name="chain">Chain to work with</param>
        public TezosRpc(Uri baseUri, TimeSpan requestTimeout, Chain chain)
            : this(new HttpClientHandler(), baseUri, requestTimeout, chain)
        {
        }

        /// <summary>
        /// Create a TezosRpc instance.
        /// </summary>
        /// <param name="handler">HTTP message handler</param>
        /// <param name="baseUri">Base URI of node</param>
        /// <param name="requestTimeout">Request timeout</param>
        /// <param name="chain">Chain to work with</param>
        public TezosRpc(HttpMessageHandler handler, Uri baseUri, TimeSpan requestTimeout, Chain chain)
        {
            Client = new RpcClient(handler, baseUri, requestTimeout);

            string chainPart = chain.ToString().ToLower();
            Blocks = new BlocksQuery(Client, $"chains/{chainPart}/blocks/");
            Inject = new InjectionQuery(Client, $"injection/");
        }

        /// <summary>
        /// Sends request and returns the json object
        /// </summary>
        /// <param name="query">Relative path to the RPC method</param>
        /// <returns></returns>
        public Task<JToken> GetAsync(string query) => Client.GetJson(query);

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
        public void Dispose()
        {
            Client?.Dispose();
        }
    }
}
