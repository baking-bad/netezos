using Dynamic.Json;
using Netezos.Rpc;

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
        public string TestSmartRollup { get; }
        public string KeyHash { get; }
        public int BigMapId { get; }
        public int TestBlockLevel { get; }

        public SettingsFixture()
        {
            lock (Crit)
            {
                var file = Environment.GetEnvironmentVariable("NETEZOS_TEST_SETTINGS") ?? "settings.json";
                var settings = DJson.Read($"../../../Rpc/{file}");

                Rpc = new TezosRpc(settings.node, 60);
                TestContract = settings.TestContract;
                TestEntrypoint = settings.TestEntrypoint;
                TestDelegate = settings.TestDelegate;
                TestInactive = settings.TestInactive;
                TestSmartRollup = settings.TestSmartRollup;
                KeyHash = settings.KeyHash;
                BigMapId = settings.BigMapId;
                TestBlockLevel = settings.TestBlockLevel;
            }
        }

        public void Dispose()
        {
            Rpc.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
