using System.Threading.Tasks;
using Dynamic.Json;
using Netezos.Rpc;
using Xunit;

namespace Netezos.Tests.Rpc
{
    public class TestHelpersQueries
    {
        readonly TezosRpc Rpc;

        public TestHelpersQueries()
        {
            var settings = DJson.Read("Rpc/settings.json");
            Rpc = new TezosRpc(settings.BaseUrl);
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
        public async Task TestHelpersForgeBlockHeader()
        {
            /*var query = Rpc.Blocks.Head.Helpers.Forge.BlockHeader;
            Assert.Equal($"chains/main/blocks/head/helpers/forge_block_header/", query.ToString());

            var res = await query.PostAsync();
            Assert.True(res is DJsonObject);*/
        }

        [Fact]
        public async Task TestHelpersForgeProtocolData()
        {
            //Add Rpc.Blocks.Head.Helpers.Forge.ProtocolData
        }

        [Fact]
        public async Task TestHelpersForgeOperations()
        {
            //Add Rpc.Blocks.Head.Helpers.Forge.Operations
        }

        [Fact]
        public async Task TestHelpersParseBlock()
        {
            //Add Rpc.Blocks.Head.Helpers.Parse.Block
        }

        [Fact]
        public async Task TestHelpersParseOperations()
        {
            //Add Rpc.Blocks.Head.Helpers.Parse.Operations
        }

        [Fact]
        public async Task TestHelpersPreapplyBlock()
        {
            //Add Rpc.Blocks.Head.Helpers.Preapply.Block
        }

        [Fact]
        public async Task TestHelpersPreapplyOperations()
        {
            //Add Rpc.Blocks.Head.Helpers.Preapply.Operations
        }

        [Fact]
        public async Task TestHelpersScriptsPackData()
        {
            //Add Rpc.Blocks.Head.Helpers.Scripts.PackData
        }

        [Fact]
        public async Task TestHelpersScriptsRunCode()
        {
            //Add Rpc.Blocks.Head.Helpers.Scripts.RunCode
        }

        [Fact]
        public async Task TestHelpersScriptsRunOperation()
        {
            //Add Rpc.Blocks.Head.Helpers.Scripts.RunOperation
        }

        [Fact]
        public async Task TestHelpersScriptsTraceCode()
        {
            //Add Rpc.Blocks.Head.Helpers.Scripts.TraceCode
        }

        [Fact]
        public async Task TestHelpersScriptsTypeCheckCode()
        {
            //Add Rpc.Blocks.Head.Helpers.Scripts.TypeCheckCode
        }

        [Fact]
        public async Task TestHelpersScriptsTypeCheckData()
        {
            //Add Rpc.Blocks.Head.Helpers.Scripts.TypeCheckData
        }
    }
}
