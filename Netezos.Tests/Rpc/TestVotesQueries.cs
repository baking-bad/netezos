using System.Threading.Tasks;
using Dynamic.Json;
using Netezos.Rpc;
using Xunit;

namespace Netezos.Tests.Rpc
{
    public class TestVotesQueries : IClassFixture<SettingsFixture>
    {
        readonly TezosRpc Rpc;

        public TestVotesQueries(SettingsFixture settings)
        {
            Rpc = settings.Rpc;
        }

        [Fact]
        public async Task TestVotesBallotList()
        {
            // Returns 404 for the SmartPy Node
            var query = Rpc.Blocks.Head.Votes.BallotList;
            Assert.Equal("chains/main/blocks/head/votes/ballot_list", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);
        }

        [Fact]
        public async Task TestVotesBallots()
        {
            // Returns 404 for the SmartPy Node
            var query = Rpc.Blocks.Head.Votes.Ballots;
            Assert.Equal("chains/main/blocks/head/votes/ballots", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonObject);
        }

        [Fact]
        public async Task TestVotesCurrentProposal()
        {
            // Returns 404 for the SmartPy Node
            var query = Rpc.Blocks.Head.Votes.CurrentProposals;
            Assert.Equal($"chains/main/blocks/head/votes/current_proposal", query.ToString());

            var res = await query.GetAsync();
            if (res != null)
            {
                Assert.True(res is DJsonValue);
            }
        }

        [Fact]
        public async Task TestVotesCurrentQuorum()
        {
            // Returns 404 for the SmartPy Node
            var query = Rpc.Blocks.Head.Votes.CurrentQuorum;
            Assert.Equal("chains/main/blocks/head/votes/current_quorum", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue); ;
        }

        [Fact]
        public async Task TestVotesListings()
        {
            // Returns 404 for the SmartPy Node
            var query = Rpc.Blocks.Head.Votes.Listings;
            Assert.Equal("chains/main/blocks/head/votes/listings", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);
        }

        [Fact]
        public async Task TestVotesProposals()
        {
            // Returns 404 for the SmartPy Node
            var query = Rpc.Blocks.Head.Votes.Proposals;
            Assert.Equal("chains/main/blocks/head/votes/proposals", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);
        }
    }
}
