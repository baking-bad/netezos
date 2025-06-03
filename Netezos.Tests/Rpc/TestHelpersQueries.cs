using Dynamic.Json;
using Netezos.Rpc;
using Xunit;

namespace Netezos.Tests.Rpc
{
    public class TestHelpersQueries : IClassFixture<SettingsFixture>
    {
        readonly TezosRpc Rpc;
        readonly string TestDelegate;

        public TestHelpersQueries(SettingsFixture settings)
        {
            Rpc = settings.Rpc;
            TestDelegate = settings.TestDelegate;
        }

        [Fact]
        public async Task TestHelpersBakingRights()
        {
            var query = Rpc.Blocks.Head.Helpers.BakingRights;
            Assert.Equal($"chains/main/blocks/head/helpers/baking_rights", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);
        }

        [Fact]
        public async Task TestHelpersAttestationRights()
        {
            var query = Rpc.Blocks.Head.Helpers.AttestationRights;
            Assert.Equal($"chains/main/blocks/head/helpers/attestation_rights", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);
        }

        [Fact]
        public async Task TestHelpersCurrentLevel()
        {
            var query = Rpc.Blocks.Head.Helpers.CurrentLevel;
            Assert.Equal($"chains/main/blocks/head/helpers/current_level", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonObject);
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
        public async Task TestHelpersLevelsInCurrentCycle()
        {
            var query = Rpc.Blocks.Head.Helpers.LevelsInCurrentCycle;
            Assert.Equal("chains/main/blocks/head/helpers/levels_in_current_cycle", query.ToString());
            
            var res = await query.GetAsync();
            Assert.True(res is DJsonObject);
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
        public void TestHelpersScriptsRunScriptView()
        {
            var query = Rpc.Blocks.Head.Helpers.Scripts.RunScriptView;
            Assert.Equal("chains/main/blocks/head/helpers/scripts/run_script_view/", query.ToString());
        }

        [Fact]
        public void TestHelpersScriptsSimulateOperation()
        {
            var query = Rpc.Blocks.Head.Helpers.Scripts.SimulateOperation;
            Assert.Equal("chains/main/blocks/head/helpers/scripts/simulate_operation/", query.ToString());
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

        [Fact]
        public async Task TestHelpersValidators()
        {
            var query = Rpc.Blocks.Head.Helpers.Validators;
            Assert.Equal("chains/main/blocks/head/helpers/validators", query.ToString());
            
            var res = await query.GetAsync(TestDelegate);
            Assert.True(res is DJsonArray);
        }

        [Fact]
        public async Task TestHelperConsecutiveRoundZero()
        {
            var query = Rpc.Blocks.Head.Helpers.ConsecutiveRoundZero;
            Assert.Equal("chains/main/blocks/head/helpers/consecutive_round_zero", query.ToString());
            
            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }
    }
}
