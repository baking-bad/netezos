using Dynamic.Json;
using Netezos.Rpc;
using Xunit;

namespace Netezos.Tests.Rpc
{
    public class TestHelpersQueries(SettingsFixture settings) : IClassFixture<SettingsFixture>
    {
        readonly TezosRpc Rpc = settings.Rpc;
        readonly string TestDelegate = settings.TestDelegate;

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

        [Fact]
        public async Task TestHelperTotalBakingPower()
        {
            var query = Rpc.Blocks.Head.Helpers.TotalBakingPower;
            Assert.Equal("chains/main/blocks/head/helpers/total_baking_power", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestHelpersAllBakersAttestActivationLevel()
        {
            var query = Rpc.Blocks.Head.Helpers.AllBakersAttestActivationLevel;
            Assert.Equal("chains/main/blocks/head/helpers/all_bakers_attest_activation_level", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonObject || res == null);
        }

        [Fact]
        public async Task TestHelpersBakingPowerDistributionForCurrentCycle()
        {
            var query = Rpc.Blocks.Head.Helpers.BakingPowerDistributionForCurrentCycle;
            Assert.Equal("chains/main/blocks/head/helpers/baking_power_distribution_for_current_cycle", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);
        }
        [Fact]
        public async Task TestHelpersDecodeDalAttestation()
        {
            var query = Rpc.Blocks.Head.Helpers.DecodeDalAttestation[0];
            Assert.Equal("chains/main/blocks/head/helpers/decode_dal_attestation/0/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);
        }
        [Fact]
        public async Task TestHelpersEncodeDalAttestation()
        {
            var query = Rpc.Blocks.Head.Helpers.EncodeDalAttestation;
            Assert.Equal("chains/main/blocks/head/helpers/encode_dal_attestation/", query.ToString());

            var res = await query.PostAsync(new[]
            {
                new { lag_index = 0, slot_indices = Array.Empty<int>() }
            });
            Assert.True(res is DJsonValue);
        }
        [Fact]
        public async Task TestHelpersForgeBlsConsensusOperations()
        {
            var query = Rpc.Blocks.Head.Helpers.Forge.BlsConsensusOperations;
            Assert.Equal("chains/main/blocks/head/helpers/forge/bls_consensus_operations/", query.ToString());

            var branch = (string)await Rpc.Blocks.Head.Hash.GetAsync();
            var res = await query.PostAsync(branch, new List<object>
            {
                new
                {
                    kind = "attestation",
                    slot = 0,
                    level = 1,
                    round = 0,
                    block_payload_hash = "vh1g87ZG6scSYxKhspAUzprQVuLAyoa5qMBKcUfjgnQGnFb3dJcG"
                }
            });
            Assert.True(res is DJsonObject);
        }
        [Fact]
        public async Task TestHelpersForgeSignedOperations()
        {
            var query = Rpc.Blocks.Head.Helpers.Forge.SignedOperations;
            Assert.Equal("chains/main/blocks/head/helpers/forge/signed_operations/", query.ToString());

            var branch = (string)await Rpc.Blocks.Head.Hash.GetAsync();
            var res = await query.PostAsync(branch, new List<object>
            {
                new
                {
                    kind = "attestation",
                    slot = 0,
                    level = 1,
                    round = 0,
                    block_payload_hash = "vh1g87ZG6scSYxKhspAUzprQVuLAyoa5qMBKcUfjgnQGnFb3dJcG"
                }
            });
            Assert.True(res is DJsonValue);
        }
        [Fact]
        public async Task TestHelpersTz4BakerNumberRatio()
        {
            var query = Rpc.Blocks.Head.Helpers.Tz4BakerNumberRatio;
            Assert.Equal("chains/main/blocks/head/helpers/tz4_baker_number_ratio", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }
    }
}
