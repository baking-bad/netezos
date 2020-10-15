using System.Threading.Tasks;
using Dynamic.Json;
using Netezos.Rpc;
using Xunit;

namespace Netezos.Tests.Rpc
{
    public class TestBlocksQueries
    {
        readonly TezosRpc Rpc;

        public TestBlocksQueries()
        {
            var settings = DJson.Read("Rpc/settings.json");
            Rpc = new TezosRpc(settings.BaseUrl);
        }

        [Fact]
        public async Task TestBlockHash()
        {
            string hash = await Rpc.Blocks.Head.Hash.GetAsync();

            Assert.NotNull(hash);
            Assert.True(hash.Length == 51);
        }

        [Fact]
        public async Task TestBlockHeader()
        {
            var header = await Rpc.Blocks.Head.Header.GetAsync();

            Assert.NotNull(header);
        }

        [Fact]
        public async Task TestBlockHeaderProtocolData()
        {
            var protocolData = await Rpc.Blocks.Head.Header.ProtocolData.GetAsync();

            Assert.NotNull(protocolData);
        }

        [Fact]
        public async Task TestBlockHeaderProtocolDataRaw()
        {
            var pdRaw = await Rpc.Blocks.Head.Header.ProtocolData.Raw.GetAsync();

            Assert.NotNull(pdRaw);
        }

        [Fact]
        public async Task TestBlockHeaderShell()
        {
            var headerShell = await Rpc.Blocks.Head.Header.Shell.GetAsync();

            Assert.NotNull(headerShell);
        }

        [Fact]
        public async Task TestBlockHeaderRaw()
        {
            var headerRaw = await Rpc.Blocks.Head.Header.Raw.GetAsync();

            Assert.NotNull(headerRaw);
        }

        [Fact]
        public async Task TestBlockMetadata()
        {
            var metaData = await Rpc.Blocks.Head.Metadata.GetAsync();

            Assert.NotNull(metaData);
        }

        [Fact]
        public async Task TestBlockLiveBlocks()
        {
            var liveBlocks = await Rpc.Blocks.Head.LiveBlocks.GetAsync();

            Assert.NotNull(liveBlocks);
            Assert.True(liveBlocks.Count >= 0);
        }

        [Fact]
        public async Task TestBlockOperationHashaes()
        {
            var OperationHashes = await Rpc.Blocks.Head.OperationsHashes.GetAsync();

            Assert.NotNull(OperationHashes);
            Assert.True(OperationHashes.Count >= 0);
        }

        [Fact]
        public async Task TestBlockOperations()
        {
            var operation = await Rpc.Blocks.Head.Operations.GetAsync();

            Assert.NotNull(operation);
            Assert.True(operation.Count >= 0);
        }
    }
}
