using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Dynamic.Json;
using Netezos.Forging.Models;
using Netezos.Keys;
using Netezos.Rpc;
using Netezos.Sandbox;
using Xunit;

namespace Netezos.Tests.Rpc
{
    public class SettingsFixture : IDisposable, IAsyncLifetime
    {
        static readonly object Crit = new object();

        public TezosRpc Rpc { get; }
        public SandboxService SandboxService { get; }

        public Key ActiveKey { get; }
        public string TestContract { get; }
        public string TestDelegate { get; }
        public string TestInactive { get; }
        private NodeContainer NodeContainer { get; }
        private int HealthCheckTimeout { get; }

        public SettingsFixture()
        {
            lock (Crit)
            {
                var settings = DJson.Read("../../../Rpc/settings.json");
                var node = GetActiveNodeConfig(settings);

                Rpc = new TezosRpc($"{node.host}:{node.port}", 60);

                
                if (node.type.ToString().Equals("internal"))
                {
                    NodeContainer = new NodeContainer(node.imageName, node.tag, node.port);

                    var headerConfig = node.header;
                    var keys = JsonSerializer.Deserialize<Dictionary<string, string>>(headerConfig.keys);

                    SandboxService = new SandboxService(
                        Rpc, 
                        headerConfig.protocol,
                        keys,
                        JsonSerializer.Deserialize<ProtocolParametersContent>(
                            headerConfig.protocolParameters.ToString())
                        );
                        
                    HealthCheckTimeout = node.healthCheckOnStartedTimeout;
                }

                ActiveKey = Key.FromMnemonic(new Mnemonic(new List<string>()
                {
                    "arctic", "blame", "brush", "economy", "solar", "swallow", "canvas", "live", "vote", "two", "post", "neutral", "spare", "split", "fall",
                }), "nbhcylbg.xllfjgrk@tezos.example.org", "ZuPOpZgMNM");

                TestContract = settings.TestContract;
                TestDelegate = settings.TestDelegate;
                TestInactive = settings.TestInactive;
            }
        }

        private async Task<bool> HealthCheckResultAsync()
        {
            try
            {
                var response = await Rpc.GetAsync("version/");
                return response.version != null;
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }

        public void Dispose() => Rpc.Dispose();

        public async Task InitializeAsync()
        {
            if (NodeContainer != null)
            {
                await NodeContainer.Container.StartAsync();

                while (!await HealthCheckResultAsync())
                {
                    Thread.Sleep(TimeSpan.FromSeconds(HealthCheckTimeout));
                }

                if (SandboxService != null)
                {
                    var branch = await SandboxService.Header.ActivateProtocol("dictator").Fill("genesis").Sign.InjectBlock.CallAsync();
                    await SandboxService.Header.BakeBlock("bootstrap1").Fill("head").Work.Sign.InjectBlock.CallAsync();

                }
            }
        }

        private static dynamic GetActiveNodeConfig(dynamic settings)
        {
            string activeNode = settings.active;

            var nodes = settings.nodes as DJsonArray;

            var node = nodes?.FirstOrDefault(x =>
            {
                string name = ((dynamic)x).name;
                return name.Equals(activeNode);
            });

            return ((dynamic)node)?.config;
        }

        public async Task DisposeAsync()
        {
            if (NodeContainer != null) 
                await NodeContainer.Container.StopAsync();
            SandboxService?.Dispose();
            Rpc.Dispose();
        }
    }

    /// <summary>
    /// Singleton instance between all tests
    /// See https://xunit.net/docs/shared-context#collection-fixture
    /// </summary>
    [CollectionDefinition(CollectionName)]
    public class SettingsCollection : ICollectionFixture<SettingsFixture>
    {
        public const string CollectionName = nameof(SettingsCollection);
    }
}
