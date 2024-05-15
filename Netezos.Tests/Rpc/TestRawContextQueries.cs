using System.Threading.Tasks;
using Dynamic.Json;
using Netezos.Rpc;
using Xunit;

namespace Netezos.Tests.Rpc
{
    public class TestRawContextQueries : IClassFixture<SettingsFixture>
    {
        readonly TezosRpc Rpc;
        readonly string TestContract;
        readonly string TestDelegate;
        readonly string TestInactive;

        public TestRawContextQueries(SettingsFixture settings)
        {
            Rpc = settings.Rpc;
            TestContract = settings.TestContract;
            TestDelegate = settings.TestDelegate;
            TestInactive = settings.TestInactive;
        }

        [Fact]
        public async Task TestRawContext()
        {
            var query = Rpc.Blocks.Head.Context.Raw;
            Assert.Equal("chains/main/blocks/head/context/raw/json/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonObject);
        }

        [Fact]
        public async Task TestRawContextBigMaps()
        {
            var query = Rpc.Blocks.Head.Context.Raw.BigMaps;
            Assert.Equal("chains/main/blocks/head/context/raw/json/big_maps/index/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);

            var query2 = Rpc.Blocks.Head.Context.Raw.BigMaps[res[0]];
            Assert.Equal($"chains/main/blocks/head/context/raw/json/big_maps/index/{res[0]}/", query2.ToString());

            var res2 = await query2.GetAsync();
            Assert.True(res2 is DJsonObject);

            var query3 = Rpc.Blocks.Head.Context.Raw.BigMaps[(int)res[0]].Contents;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/big_maps/index/{res[0]}/contents/", query3.ToString());

            var res3 = await query3.GetAsync();
            Assert.True(res3 is DJsonArray);

            /*var query4 = Rpc.Blocks.Head.Context.Raw.BigMaps[(int)res[0]].Contents[res3[0]];
            Assert.Equal($"chains/main/blocks/head/context/raw/json/big_maps/index/{res[0]}/contents/{res3[0]}/", query4.ToString());

            var res4 = await query4.GetAsync();
            Assert.True(res4 is DJsonObject);*/
        }

        [Fact]
        public async Task TestRawContextCommitments()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Commitments;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/commitments/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);
        }

        [Fact]
        public async Task TestRawContextGlobalCounter()
        {
            var query = Rpc.Blocks.Head.Context.Raw.GlobalCounter;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/contracts/global_counter/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestRawContextContract()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Contracts[TestDelegate];
            Assert.Equal($"chains/main/blocks/head/context/raw/json/contracts/index/{TestDelegate}/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonObject);
        }

        [Fact]
        public async Task TestRawContextContractBalance()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Contracts[TestDelegate].Balance;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/contracts/index/{TestDelegate}/balance/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public void TestRawContextContractBigMap()
        {
            var query = Rpc.Blocks[123].Context.Raw.Contracts[TestContract].BigMap; // specific level is required
            Assert.Equal($"chains/main/blocks/123/context/raw/json/contracts/index/{TestContract}/big_map/", query.ToString());

            //var res = await query.GetAsync();
            //Assert.True(res is DJsonArray);
        }

        [Fact]
        public async Task TestRawContextContractCode()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Contracts[TestContract].Code;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/contracts/index/{TestContract}/code/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);
        }

        [Fact]
        public async Task TestRawContextContractCounter()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Contracts[TestDelegate].Counter;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/contracts/index/{TestDelegate}/counter/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestRawContextContractDelegate()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Contracts[TestDelegate].Delegate;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/contracts/index/{TestDelegate}/delegate/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestRawContextContractDelegateDesactivation()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Contracts[TestDelegate].DelegateDesactivation;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/contracts/index/{TestDelegate}/delegate_desactivation/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestRawContextContractDelegated()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Contracts[TestDelegate].Delegated;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/contracts/index/{TestDelegate}/delegated/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);
        }

        [Fact]
        public async Task TestRawContextContractManager()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Contracts[TestDelegate].Manager;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/contracts/index/{TestDelegate}/manager/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestRawContextContractPaidBytes()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Contracts[TestContract].PaidBytes;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/contracts/index/{TestContract}/paid_bytes/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestRawContextContractStorage()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Contracts[TestContract].Storage;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/contracts/index/{TestContract}/storage/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);
        }

        [Fact]
        public async Task TestRawContextContractUsedBytes()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Contracts[TestContract].UsedBytes;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/contracts/index/{TestContract}/used_bytes/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestRawContextCycles()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Cycles;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/cycle/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);
        }

        [Fact]
        public void TestRawContextCycle()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Cycles[123]; // specific cycle is required
            Assert.Equal($"chains/main/blocks/head/context/raw/json/cycle/123/", query.ToString());

            //var res = await query.GetAsync();
            //Assert.True(res is DJsonObject);
        }

        [Fact]
        public void TestRawContextCycleLastRoll()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Cycles[123].LastRoll; // specific cycle is required
            Assert.Equal($"chains/main/blocks/head/context/raw/json/cycle/123/last_roll/", query.ToString());

            //var res = await query.GetAsync();
            //Assert.True(res is DJsonArray);
        }

        [Fact]
        public void TestRawContextCycleNonces()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Cycles[123].Nonces; // specific cycle is required
            Assert.Equal($"chains/main/blocks/head/context/raw/json/cycle/123/nonces/", query.ToString());

            //var res = await query.GetAsync();
            //Assert.True(res is DJsonArray);
        }

        [Fact]
        public void TestRawContextRandomSeed()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Cycles[123].RandomSeed; // specific cycle is required
            Assert.Equal($"chains/main/blocks/head/context/raw/json/cycle/123/random_seed/", query.ToString());

            //var res = await query.GetAsync();
            //Assert.True(res is DJsonValue);
        }

        [Fact]
        public void TestRawContextRollSnapshot()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Cycles[123].RollSnapshot; // specific cycle is required
            Assert.Equal($"chains/main/blocks/head/context/raw/json/cycle/123/roll_snapshot/", query.ToString());

            //var res = await query.GetAsync();
            //Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestRawContextDelegates()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Delegates;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/delegates/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);

            var query2 = Rpc.Blocks.Head.Context.Raw.Delegates[TestDelegate];
            Assert.Equal($"chains/main/blocks/head/context/raw/json/delegates/{TestDelegate}/", query2.ToString());

            var res2 = await query2.GetAsync();
            Assert.True(res2 is DJsonValue);
        }

        [Fact]
        public async Task TestRawContextRampUp()
        {
            var query = Rpc.Blocks.Head.Context.Raw.RampUp;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/ramp_up/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonObject);
        }

        [Fact]
        public async Task TestRawContextVotes()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Votes;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/votes/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonObject);
        }

        [Fact]
        public async Task TestRawContextVotesBallots()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Votes.Ballots;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/votes/ballots/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);
        }

        [Fact]
        public void TestRawContextVotesCurrentQuorum()
        {
            var query = Rpc.Blocks[123].Context.Raw.Votes.CurrentQuorum; // specific level is required
            Assert.Equal($"chains/main/blocks/123/context/raw/json/votes/current_quorum/", query.ToString());

            //var res = await query.GetAsync();
            //Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestRawContextVotesListings()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Votes.Listings;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/votes/listings/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);
        }
        
        [Fact]
        public async Task TestRawContextVotesProposals()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Votes.Proposals;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/votes/proposals/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);
        }

        [Fact]
        public async Task TestRawContextVotesProposalsCount()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Votes.ProposalsCount;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/votes/proposals_count/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);
        }
    }
}
