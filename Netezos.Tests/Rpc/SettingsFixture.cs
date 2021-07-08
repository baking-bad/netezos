using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Dynamic.Json;
using Netezos.Forging.Sandbox;
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
        private SandboxBlockHeaderService SandboxBlockHeader { get; }
        private int HealthCheckTimeout { get; }

        public SettingsFixture()
        {
            lock (Crit)
            {
                var settings = DJson.Read("../../../Rpc/settings.json");

                NodeContainer = new NodeContainer(settings.sandboxNode.imageName, settings.sandboxNode.tag, settings.sandboxNode.port);
                Rpc = new TezosRpc($"{settings.sandboxNode.host}:{settings.sandboxNode.port}", 60);
                SandboxBlockHeader = new SandboxBlockHeaderService(Rpc);
                HealthCheckTimeout = settings.sandboxNode.healthCheckOnStartedTimeout;

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
            catch (HttpRequestException e)
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

            await SandboxBlockHeader.ActivationTest();
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
