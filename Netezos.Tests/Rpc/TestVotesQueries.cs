using System.Threading.Tasks;
using Dynamic.Json;
using Netezos.Rpc;
using Xunit;

namespace Netezos.Tests.Rpc
{
    public class TestVotesQueries
    {
        readonly TezosRpc Rpc;

        public TestVotesQueries()
        {
            var settings = DJson.Read("Rpc/settings.json");
            Rpc = new TezosRpc(settings.BaseUrl);
        }

        [Fact]
        public async Task TestVotesBallotList()
        {
            var query = Rpc.Blocks.Head.Votes.BallotList;
            Assert.Equal("chains/main/blocks/head/votes/ballot_list/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);
        }

        [Fact]
        public async Task TestVotesBallots()
        {
            var ballot = await Rpc.Blocks.Head.Votes.Ballots.GetAsync();

            Assert.NotNull(ballot);
        }

        [Fact]
        public async Task TestVotesCurrentPeriodKind()
        {
            var currentPK = await Rpc.Blocks.Head.Votes.CurrentPeriodKind.GetAsync();

            Assert.NotNull(currentPK);
        }

        [Fact]
        public async Task TestVotesCurrentProposal()
        {
            var currentProposal = await Rpc.Blocks.Head.Votes.CurrentProposals.GetAsync();

            Assert.NotNull(currentProposal);
        }

        [Fact]
        public async Task TestVotesCurrentQuorum()
        {
            var currentQuorum = await Rpc.Blocks.Head.Votes.CurrentQuorum.GetAsync();

            Assert.NotNull(currentQuorum);
        }

        [Fact]
        public async Task TestVotesListings()
        {
            var listings = await Rpc.Blocks.Head.Votes.Listings.GetAsync();

            Assert.NotNull(listings);
            Assert.True(listings.Count >= 0);
        }

        [Fact]
        public async Task TestVotesProposals()
        {
            var proposals = await Rpc.Blocks.Head.Votes.Proposals.GetAsync();

            Assert.NotNull(proposals);
            Assert.True(proposals.Count >= 0);
        }
    }
}
