using Dynamic.Json;
using Netezos.Rpc;
using System;

namespace Netezos.Tests.Rpc
{
    public class SettingsFixture : IDisposable
    {
        static readonly object Crit = new object();

        public TezosRpc Rpc { get; }
        public string TestContract { get; }
        public string TestDelegate { get; }
        public string TestInactive { get; }

        public SettingsFixture()
        {
            lock (Crit)
            {
                var settings = DJson.Read("../../../Rpc/settings.json");

                Rpc = new TezosRpc(settings.node, 60);
                TestContract = settings.TestContract;
                TestDelegate = settings.TestDelegate;
                TestInactive = settings.TestInactive;
            }
        }

        public void Dispose() => Rpc.Dispose();
    }
}
