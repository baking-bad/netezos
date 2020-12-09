using System.Threading.Tasks;
using Dynamic.Json;
using Netezos.Rpc;
using Xunit;

namespace Netezos.Tests.Rpc
{
    public class TestBlocksQueries : IClassFixture<SettingsFixture>
    {
        readonly TezosRpc Rpc;

        public TestBlocksQueries(SettingsFixture settings)
        {
            Rpc = settings.Rpc;
        }

        [Fact]
        public async Task TestBlockHash()
        {
            var query =  Rpc.Blocks.Head.Hash;
            Assert.Equal($"chains/main/blocks/head/hash/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestBlockHeader()
        {
            var query = Rpc.Blocks.Head.Header;
            Assert.Equal($"chains/main/blocks/head/header/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonObject);
        }

        [Fact]
        public async Task TestBlockHeaderProtocolData()
        {
            var query = Rpc.Blocks.Head.Header.ProtocolData;
            Assert.Equal($"chains/main/blocks/head/header/protocol_data/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonObject);
        }

        [Fact]
        public async Task TestBlockHeaderProtocolDataRaw()
        {
            var query =  Rpc.Blocks.Head.Header.ProtocolData.Raw;
            Assert.Equal($"chains/main/blocks/head/header/protocol_data/raw/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestBlockHeaderShell()
        {
            var query = Rpc.Blocks.Head.Header.Shell;
            Assert.Equal($"chains/main/blocks/head/header/shell/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonObject);
        }

        [Fact]
        public async Task TestBlockHeaderRaw()
        {
            var query = Rpc.Blocks.Head.Header.Raw;
            Assert.Equal($"chains/main/blocks/head/header/raw/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestBlockMetadata()
        {
            var query = Rpc.Blocks.Head.Metadata;
            Assert.Equal($"chains/main/blocks/head/metadata/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonObject);
        }

        [Fact]
        public async Task TestBlockLiveBlocks()
        {
            var query = Rpc.Blocks.Head.LiveBlocks;
            Assert.Equal($"chains/main/blocks/head/live_blocks/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);
        }

        [Fact]
        public async Task TestBlockOperationHashaes()
        {
            var query = Rpc.Blocks.Head.OperationsHashes;
            Assert.Equal($"chains/main/blocks/head/operation_hashes/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);
        }

        [Fact]
        public async Task TestBlockOperations()
        {
            var query = Rpc.Blocks.Head.Operations;
            Assert.Equal($"chains/main/blocks/head/operations/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);
        }
    }
}
