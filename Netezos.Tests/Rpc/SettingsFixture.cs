using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Dynamic.Json;
using Netezos.Forging.Sandbox.Header;
using Netezos.Rpc;
using Xunit;

namespace Netezos.Tests.Rpc
{
    public class SettingsFixture : IDisposable, IAsyncLifetime
    {
        static readonly object Crit = new object();

        public TezosRpc Rpc { get; }
        public string TestContract { get; }
        public string TestDelegate { get; }
        public string TestInactive { get; }
        private NodeContainer NodeContainer { get; }
        private HeaderClient HeaderClient { get; }
        private int HealthCheckTimeout { get; }

        public SettingsFixture()
        {
            lock (Crit)
            {
                var settings = DJson.Read("../../../Rpc/settings.json");
                var node = GetActiveNodeConfig(settings);
                var headerConfig = node.header;

                NodeContainer = new NodeContainer(node.imageName, node.tag, node.port);
                Rpc = new TezosRpc($"{node.host}:{node.port}", 60);

                var keys = JsonSerializer.Deserialize<Dictionary<string, string>>(headerConfig.keys);

                HeaderClient = new HeaderClient(
                    Rpc,
                    headerConfig.protocol,
                    keys,
                    headerConfig.protocolParameters);

                HealthCheckTimeout = node.healthCheckOnStartedTimeout;

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
            await NodeContainer.Container.StartAsync();

            while (!await HealthCheckResultAsync())
            {
                Thread.Sleep(TimeSpan.FromSeconds(HealthCheckTimeout));
            }

            if (HeaderClient != null)
            {
                await HeaderClient.ActivateProtocol("dictator").Fill().Sign.InjectBlock.CallAsync();
                await HeaderClient.BakeBlock("bootstrap1").Fill().Work.Sign.InjectBlock.CallAsync();
            }
        }

        private static dynamic GetActiveNodeConfig(dynamic settings)
        {
            string activeNode = settings.active;

            var nodes = settings.nodes as DJsonArray;

            var node = nodes?.FirstOrDefault(x =>
            {
                string type = ((dynamic)x).type;
                return type.Equals(activeNode);
            });

            return ((dynamic)node)?.config;
        }

        public async Task DisposeAsync()
        {
            await NodeContainer.Container.StopAsync();
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
