using Dynamic.Json;
using Netezos.Rpc;
using System;

namespace Netezos.Tests.Rpc
{
    public class SettingsFixture : IDisposable
    {
        static readonly object Crit = new();

        public TezosRpc Rpc { get; }
        public string TestContract { get; }
        public string TestEntrypoint { get; }
        public string TestDelegate { get; }
        public string TestInactive { get; }
        public string KeyHash { get; }
        public int BigMapId { get; }

        public SettingsFixture()
        {
            lock (Crit)
            {
                var settings = DJson.Read("../../../Rpc/settings.json");

                Rpc = new TezosRpc(settings.node, 60);
                TestContract = settings.TestContract;
                TestEntrypoint = settings.TestEntrypoint;
                TestDelegate = settings.TestDelegate;
                TestInactive = settings.TestInactive;
                KeyHash = settings.KeyHash;
                BigMapId = settings.BigMapId;
            }
        }

        public void Dispose()
        {
            Rpc.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
