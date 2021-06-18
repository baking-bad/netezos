using Dynamic.Json;
using Netezos.Rpc;
using System;
using Xunit;

namespace Netezos.Tests.Rpc
{
    public class SettingsFixture : IDisposable
    {
        static readonly object Crit = new object();

        public TezosRpcSandbox Rpc { get; }
        public string TestContract { get; }
        public string TestDelegate { get; }
        public string TestInactive { get; }

        public SettingsFixture()
        {
            lock (Crit)
            {
                var settings = DJson.Read("../../../Rpc/settings.json");

                Rpc = new TezosRpcSandbox(settings.node, 60);
                TestContract = settings.TestContract;
                TestDelegate = settings.TestDelegate;
                TestInactive = settings.TestInactive;
            }
        }

        public void Dispose() => Rpc.Dispose();
    }

    /// <summary>
    /// Singleton instance between all tests
    /// See https://xunit.net/docs/shared-context#collection-fixture
    /// </summary>
    [CollectionDefinition("Settings")]
    public class SettingsCollection : ICollectionFixture<SettingsFixture>
    {
        
    }
}
