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
            var BakingRights = await Rpc.Blocks.Head.Helpers.BakingRights.GetAsync();

            Assert.NotNull(BakingRights);
            Assert.True(BakingRights.Count >= 0);
        }

        [Fact]
        public async Task TestHelpersEndorsingRights()
        {
            var endorsingRights = await Rpc.Blocks.Head.Helpers.EndorsingRights.GetAsync();

            Assert.NotNull(endorsingRights);
            Assert.True(endorsingRights.Count >= 0);
        }

        [Fact]
        public async Task TestHelpersForgeBlockHeader()
        {
            //Add Rpc.Blocks.Head.Helpers.Forge.BlockHeader
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
