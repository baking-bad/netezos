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
using Netezos.Sandbox.Models;
using Xunit;

namespace Netezos.Tests.Startup
{
    public class SettingsFixture : IDisposable, IAsyncLifetime
    {
        static readonly object Crit = new object();

        public TezosRpc Rpc { get; }
        public SandboxService SandboxService { get; }
        public KeyStore KeyStore { get; }
        public string TestContract { get; }
        public string TestDelegate { get; }
        public string TestInactive { get; }
        private NodeContainer NodeContainer { get; }
        private int HealthCheckTimeout { get; }

        public SettingsFixture()
        {
            lock (Crit)
            {
                var settings = DJson.Read("../../../Startup/settings.json");
                var node = GetActiveNodeConfig(settings);

                Rpc = new TezosRpc($"{node.host}:{node.port}", 60);

                if (node.type.ToString().Equals("internal"))
                {
                    NodeContainer = new NodeContainer(node.imageName, node.tag, node.port);

                    KeyStore = LoadKeys(node);

                    SandboxService = new SandboxService(
                        Rpc, 
                        node.header.protocol,
                        KeyStore,
                        JsonSerializer.Deserialize<ProtocolParametersContent>(
                            node.header.protocolParameters.ToString()),
                        JsonSerializer.Deserialize<SandboxConstants>(
                            node.sandboxConstants.ToString())
                        );

                    HealthCheckTimeout = node.healthCheckOnStartedTimeout;
                }

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
                    await SandboxService.ActivateProtocol("dictator", "genesis");
                    await SandboxService.BakeBlock("bootstrap2", "head");
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

        private KeyStore LoadKeys(dynamic node)
        {
            var keys = JsonSerializer.Deserialize<Dictionary<string, string>>(node.header.keys);
            CommitmentKey commitments = CommitmentKey.FromMnemonic(
                new Mnemonic(JsonSerializer.Deserialize<List<string>>(node.sandboxCommitment.mnemonic)), 
                node.sandboxCommitment.email,
                node.sandboxCommitment.password,
                node.sandboxCommitment.secret.ToString());
            return new KeyStore(keys, commitments);
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
