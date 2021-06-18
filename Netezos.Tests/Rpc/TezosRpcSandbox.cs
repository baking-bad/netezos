using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Dynamic.Json;
using Netezos.Rpc;
using Netezos.Rpc.Queries;

namespace Netezos.Tests.Rpc
{
    /// <summary>
    /// Wrapper for TezosRpc.
    /// Use this implementation for a large number of queries.
    /// </summary>
    public sealed class TezosRpcSandbox : IDisposable
    {
        private TezosRpc _Rpc;
        public TezosRpcSandbox(string url, int timeout)
        {
            _Rpc = new TezosRpc(url, timeout);
        }

        /// <summary>
        /// Gets the query to the blocks
        /// </summary>
        public BlocksQuery Blocks => _Rpc.Blocks;

        /// <summary>
        /// Gets the query to the injection
        /// </summary>
        public InjectionQuery Inject => _Rpc.Inject;

        /// <summary>
        /// Creates the instanse of TezosRpc
        /// </summary>
        /// <param name="uri">Base URI of the node</param>
        /// <param name="chain">Chain to work with.
        /// Note: this is not a network (mainnet or testnet), but a chain of the particular network.
        /// In 99.99% cases you likely want to use Chain.Main, because Chain.Test is only relevant during the testing phase of the Tezos voting process.</param>
        public TezosRpcSandbox(string uri, Chain chain = Chain.Main)
        {
            _Rpc = new TezosRpc(uri, chain);
        }

        /// <summary>
        /// Creates the instance of TezosRpc
        /// </summary>
        /// <param name="uri">Base URI of the node</param>
        /// <param name="timeout">Timeout in seconds for the requests</param>
        /// <param name="chain">Chain to work with.
        /// Note: this is not a network (mainnet or testnet), but a chain of the network.
        /// In 99.99% cases you likely want to use Chain.Main, because Chain.Test is only relevant during the testing phase of the Tezos voting process.</param>
        public TezosRpcSandbox(string uri, int timeout, Chain chain = Chain.Main)
        {
            _Rpc = new TezosRpc(uri, timeout, chain);
        }

        /// <summary>
        /// Creates the instance of TezosRpc
        /// </summary>
        /// <param name="client">HttpClient instanse that will be used for sending RPC requests.</param>
        /// <param name="chain">Chain to work with.
        /// Note: this is not a network (mainnet or testnet), but a chain of the network.
        /// In 99.99% cases you likely want to use Chain.Main, because Chain.Test is only relevant during the testing phase of the Tezos voting process.</param>
        public TezosRpcSandbox(HttpClient client, Chain chain = Chain.Main)
        {
            _Rpc = new TezosRpc(client, chain);
        }

        /// <summary>
        /// Wait for the node to accept requests and sends request and returns the dynamic json object
        /// </summary>
        /// <param name="query">Relative path to the RPC method</param>
        /// <returns></returns>
        public async Task<dynamic> GetAsync(string query)
        {
            while (!await HealthCheckAsync())
            {
                Thread.Sleep(2000);
            }
            return await _Rpc.GetAsync(query);
        }

        private async Task<bool> HealthCheckAsync()
        {
            var response = (DJsonObject)await _Rpc.GetAsync("version/");
            var members = response.GetDynamicMemberNames();
            return members.Any(x => x.Equals("version"));
        } 

        public void Dispose() => _Rpc?.Dispose();
    }
}