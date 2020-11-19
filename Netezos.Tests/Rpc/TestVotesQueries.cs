using System;
using System.Threading.Tasks;
using Dynamic.Json;
using Netezos.Rpc;
using Xunit;

namespace Netezos.Tests.Rpc
{
    public class TestVotesQueries
    {
        readonly TezosRpc Rpc;
        readonly int? LegacyLevel;

        public TestVotesQueries()
        {
            var settings = DJson.Read("Rpc/settings.json");
            Rpc = new TezosRpc(settings.BaseUrl);
            LegacyLevel = settings.LegacyLevel;
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
            var query = Rpc.Blocks.Head.Votes.Ballots;
            Assert.Equal("chains/main/blocks/head/votes/ballots/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonObject);
        }

        [Fact]
        public async Task TestVotesCurrentPeriodKind()
        {
            var query = Rpc.Blocks.Head.Votes.CurrentPeriodKind;
            Assert.Equal("chains/main/blocks/head/votes/current_period_kind/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestVotesCurrentProposal()
        {
            var query = Rpc.Blocks[1200000].Votes.CurrentProposals;
            Assert.Equal($"chains/main/blocks/1200000/votes/current_proposal/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestVotesCurrentQuorum()
        {
            var query = Rpc.Blocks.Head.Votes.CurrentQuorum;
            Assert.Equal("chains/main/blocks/head/votes/current_quorum/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue); ;
        }

        [Fact]
        public async Task TestVotesListings()
        {
            var query = Rpc.Blocks.Head.Votes.Listings;
            Assert.Equal("chains/main/blocks/head/votes/listings/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);
        }

        [Fact]
        public async Task TestVotesProposals()
        {
            var query = Rpc.Blocks.Head.Votes.Proposals;
            Assert.Equal("chains/main/blocks/head/votes/proposals/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);
        }
    }
}
