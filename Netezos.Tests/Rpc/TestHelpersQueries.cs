using System.Threading.Tasks;
using Dynamic.Json;
using Netezos.Rpc;
using Xunit;
using System.Collections.Generic;
using System;

namespace Netezos.Tests.Rpc
{
    public class TestHelpersQueries
    {
        readonly TezosRpc Rpc;
        readonly string TestDelegate;
        readonly string PublicKey;


        public TestHelpersQueries()
        {
            var settings = DJson.Read("Rpc/settings.json");
            Rpc = new TezosRpc(settings.BaseUrl);
            TestDelegate = settings.TestDelegate;
            PublicKey = settings.PublicKey;
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
            var query = Rpc.Blocks.Head.Helpers.Forge.BlockHeader;
            Assert.Equal("chains/main/blocks/head/helpers/forge_block_header/", 
                query.ToString());

        }

        [Fact]
        public async Task TestHelpersForgeProtocolData()
        {
            var query = Rpc.Blocks.Head.Helpers.Forge.ProtocolData;
            Assert.Equal("chains/main/blocks/head/helpers/forge/protocol_data/",
                query.ToString());

        }

        [Fact]
        public async Task TestHelpersForgeOperations()
        {
            var query = Rpc.Blocks.Head.Helpers.Forge.Operations;
            Assert.Equal("chains/main/blocks/head/helpers/forge/operations/",
                query.ToString());

            var branch = await Rpc.Blocks.Head.Hash.GetAsync<string>();
            var content = new List<object>
            {
                new
                {
                    kind = "activate_account",
                    pkh = "tz1SMAJFAuXyTJ9MttBsLdQQ4idwFAvofi65",
                    secret = "161b264d37a2b5f4103939de53a57b3dee284d38",
                }
            };

            var res = await query.PostAsync(branch, content);
            Assert.True(res is DJsonValue);

        }

        [Fact]
        public async Task TestHelpersParseBlock()
        {
            var query = Rpc.Blocks.Head.Helpers.Parse.Block;
            Assert.Equal("chains/main/blocks/head/helpers/parse/block/",
                query.ToString());

        }

        [Fact]
        public async Task TestHelpersParseOperations()
        {
            var query = Rpc.Blocks.Head.Helpers.Parse.Operations;
            Assert.Equal("chains/main/blocks/head/helpers/parse/operations/",
                query.ToString());

        }


        [Fact]
        public async Task TestHelpersPreapplyBlock()
        {
            var query = Rpc.Blocks.Head.Helpers.Preapply.Block;
            Assert.Equal("chains/main/blocks/head/helpers/preapply/block",
                query.ToString());

        }

        [Fact]
        public async Task TestHelpersPreapplyOperations()
        {
            var query = Rpc.Blocks.Head.Helpers.Preapply.Operations;
            Assert.Equal("chains/main/blocks/head/helpers/preapply/operations",
                query.ToString());
        }

        [Fact]
        public async Task TestHelpersScriptsPackData()
        {
            var query = Rpc.Blocks.Head.Helpers.Scripts.PackData;
            Assert.Equal("chains/main/blocks/head/helpers/scripts/pack_data/",
                query.ToString());
        }

        [Fact]
        public async Task TestHelpersScriptsRunCode()
        {
            var query = Rpc.Blocks.Head.Helpers.Scripts.RunCode;
            Assert.Equal("chains/main/blocks/head/helpers/scripts/run_code/",
                query.ToString());
        }

        [Fact]
        public async Task TestHelpersScriptsRunOperation()
        {
            var query = Rpc.Blocks.Head.Helpers.Scripts.RunOperation;
            Assert.Equal("chains/main/blocks/head/helpers/scripts/run_operation/",
                query.ToString());
        }

        [Fact]
        public async Task TestHelpersScriptsTraceCode()
        {
            var query = Rpc.Blocks.Head.Helpers.Scripts.TraceCode;
            Assert.Equal("chains/main/blocks/head/helpers/scripts/trace_code/",
                query.ToString());
        }

        [Fact]
        public async Task TestHelpersScriptsTypeCheckCode()
        {
            var query = Rpc.Blocks.Head.Helpers.Scripts.TypeCheckCode;
            Assert.Equal("chains/main/blocks/head/helpers/scripts/typecheck_code/",
                query.ToString());
        }

        [Fact]
        public async Task TestHelpersScriptsTypeCheckData()
        {
            var query = Rpc.Blocks.Head.Helpers.Scripts.TypeCheckData;
            Assert.Equal("chains/main/blocks/head/helpers/scripts/typecheck_data/",
                query.ToString());
        }
    }
}
