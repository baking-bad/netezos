using System.Threading.Tasks;
using Dynamic.Json;
using Netezos.Rpc;
using Xunit;

namespace Netezos.Tests.Rpc
{
    public class TestHelpersQueries : IClassFixture<SettingsFixture>
    {
        readonly TezosRpc Rpc;

        public TestHelpersQueries(SettingsFixture settings)
        {
            Rpc = settings.Rpc;
        }

        [Fact]
        public async Task TestHelpersBakingRights()
        {
            var query = Rpc.Blocks.Head.Helpers.BakingRights;
            Assert.Equal($"chains/main/blocks/head/helpers/baking_rights/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);
        }

        [Fact]
        public async Task TestHelpersEndorsingRights()
        {
            var query = Rpc.Blocks.Head.Helpers.EndorsingRights;
            Assert.Equal($"chains/main/blocks/head/helpers/endorsing_rights/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);
        }

        [Fact]
        public void TestHelpersForgeBlockHeader()
        {
            var query = Rpc.Blocks.Head.Helpers.Forge.BlockHeader;
            Assert.Equal("chains/main/blocks/head/helpers/forge_block_header/", query.ToString());
        }

        [Fact]
        public void TestHelpersForgeProtocolData()
        {
            var query = Rpc.Blocks.Head.Helpers.Forge.ProtocolData;
            Assert.Equal("chains/main/blocks/head/helpers/forge/protocol_data/", query.ToString());
        }

        [Fact]
        public void TestHelpersForgeOperations()
        {
            var query = Rpc.Blocks.Head.Helpers.Forge.Operations;
            Assert.Equal("chains/main/blocks/head/helpers/forge/operations/", query.ToString());
        }

        [Fact]
        public void TestHelpersParseBlock()
        {
            var query = Rpc.Blocks.Head.Helpers.Parse.Block;
            Assert.Equal("chains/main/blocks/head/helpers/parse/block/", query.ToString());
        }

        [Fact]
        public void TestHelpersParseOperations()
        {
            var query = Rpc.Blocks.Head.Helpers.Parse.Operations;
            Assert.Equal("chains/main/blocks/head/helpers/parse/operations/", query.ToString());
        }

        [Fact]
        public void TestHelpersPreapplyBlock()
        {
            var query = Rpc.Blocks.Head.Helpers.Preapply.Block;
            Assert.Equal("chains/main/blocks/head/helpers/preapply/block", query.ToString());
        }

        [Fact]
        public void TestHelpersPreapplyOperations()
        {
            var query = Rpc.Blocks.Head.Helpers.Preapply.Operations;
            Assert.Equal("chains/main/blocks/head/helpers/preapply/operations", query.ToString());
        }

        [Fact]
        public void TestHelpersScriptsPackData()
        {
            var query = Rpc.Blocks.Head.Helpers.Scripts.PackData;
            Assert.Equal("chains/main/blocks/head/helpers/scripts/pack_data/", query.ToString());
        }

        [Fact]
        public void TestHelpersScriptsRunCode()
        {
            var query = Rpc.Blocks.Head.Helpers.Scripts.RunCode;
            Assert.Equal("chains/main/blocks/head/helpers/scripts/run_code/", query.ToString());
        }

        [Fact]
        public void TestHelpersScriptsRunOperation()
        {
            var query = Rpc.Blocks.Head.Helpers.Scripts.RunOperation;
            Assert.Equal("chains/main/blocks/head/helpers/scripts/run_operation/", query.ToString());
        }

        [Fact]
        public void TestHelpersScriptsTraceCode()
        {
            var query = Rpc.Blocks.Head.Helpers.Scripts.TraceCode;
            Assert.Equal("chains/main/blocks/head/helpers/scripts/trace_code/", query.ToString());
        }

        [Fact]
        public void TestHelpersScriptsTypeCheckCode()
        {
            var query = Rpc.Blocks.Head.Helpers.Scripts.TypeCheckCode;
            Assert.Equal("chains/main/blocks/head/helpers/scripts/typecheck_code/", query.ToString());
        }

        [Fact]
        public void TestHelpersScriptsTypeCheckData()
        {
            var query = Rpc.Blocks.Head.Helpers.Scripts.TypeCheckData;
            Assert.Equal("chains/main/blocks/head/helpers/scripts/typecheck_data/", query.ToString());
        }
    }
}
