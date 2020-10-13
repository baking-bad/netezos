using System;
using System.Threading.Tasks;
using Xunit;

namespace Netezos.Rpc.Tests
{
    public class TestBlocks
    {
        readonly TezosRpc Rpc;

        public TestBlocks()
        {
            Rpc = new TezosRpc(new Uri("https://mainnet-tezos.giganode.io/"));
        }

        [Fact]
        public async Task TestHead()
        {
            var head = await Rpc.Blocks.Head.GetAsync();

            Assert.NotNull(head);
        }

        [Fact]
        public async Task TestHeadHash()
        {
            var hash = await Rpc.Blocks.Head.Hash.GetAsync<string>();

            Assert.NotNull(hash);
            Assert.Matches("^[0-9A-z]{51}$", hash);
        }
    }
}
