using System;
using System.Threading.Tasks;
using Netezos.Rpc;
using Xunit;

namespace Netezos.Tests.Rpc
{
    public class RpcTests
    {
        readonly TezosRpc Rpc;

        public RpcTests()
        {
            Rpc = new TezosRpc("https://mainnet-tezos.giganode.io/");
        }

        [Fact]
        public async Task TestHead()
        {
            var head = await Rpc.Blocks.Head.GetAsync();

            Assert.NotNull(head);
            Assert.Matches("^[0-9A-z]{51}$", head.Hash);
            Assert.Matches("^[0-9A-z]{15}$", head.ChainId);
            Assert.Matches("^[0-9A-z]{15}$", head.chain_id);
        }

        [Fact]
        public async Task TestHeadHash()
        {
            var hash = await Rpc.Blocks.Head.Hash.GetAsync<string>();

            Assert.NotNull(hash);
            Assert.Matches("^[0-9A-z]{51}$", hash);
        }

        [Fact]
        public async Task TestDeserialization()
        {
            var block = await Rpc.Blocks.Head.GetAsync<BlockObj>();
            var block2 = await Rpc.Blocks.Head.GetAsync<BlockObj2>();

            Assert.True(block.Metadata.ConsumedGas > 0);
            Assert.True(block2.metadata.consumed_gas > 0);
        }

        class BlockObj
        {
            public BlockMetadataObj Metadata { get; set; }
        }

        class BlockMetadataObj
        {
            public long ConsumedGas { get; set; }
        }

        class BlockObj2
        {
            public BlockMetadataObj2 metadata { get; set; }
        }

        class BlockMetadataObj2
        {
            public long consumed_gas { get; set; }
        }
    }
}
