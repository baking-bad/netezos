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
        public async Task TestRawContextActiveDelegates()
        {
            var query = Rpc.Blocks.Head.Context.Raw.ActiveDelegates;
            Assert.Equal("chains/main/blocks/head/context/raw/json/active_delegates_with_rolls/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);

            var query2 = Rpc.Blocks.Head.Context.Raw.ActiveDelegates[res[0]];
            Assert.Equal($"chains/main/blocks/head/context/raw/json/active_delegates_with_rolls/{res[0]}/", query2.ToString());

            var res2 = await query2.GetAsync();
            Assert.True(res2 is DJsonValue);
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
        public async Task TestRawContextContractChange()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Contracts[TestDelegate].Change;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/contracts/index/{TestDelegate}/change/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
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
        public void TestRawContextContractDelegatable()
        {
            //var query = Rpc.Blocks.Head.Context.Raw.Contracts[addr].Delegatable;
            //Assert.Equal($"chains/main/blocks/head/context/raw/json/contracts/index/{addr}/delegatable/", query.ToString());

            //var res = await query.GetAsync();
            //Assert.True(res is DJsonValue);
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
        public async Task TestRawContextContractInactiveDelegate()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Contracts[TestInactive].InactiveDelegate;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/contracts/index/{TestInactive}/inactive_delegate/", query.ToString());

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
        public async Task TestRawContextContractFrozenBalance()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Contracts[TestDelegate].FrozenBalance;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/contracts/index/{TestDelegate}/frozen_balance/", query.ToString());

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
        public async Task TestRawContextContractRollList()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Contracts[TestDelegate].RollList;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/contracts/index/{TestDelegate}/roll_list/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public void TestRawContextContractSpendable()
        {
            //var query = Rpc.Blocks.Head.Context.Raw.Contracts[addr].Spendable;
            //Assert.Equal($"chains/main/blocks/head/context/raw/json/contracts/index/{addr}/spendable/", query.ToString());

            //var res = await query.GetAsync();
            //Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestRawContextContractStorage()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Contracts[TestContract].Storage;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/contracts/index/{TestContract}/storage/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonObject);
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
        public async Task TestRawContextDelegatesWithFrozenBalance()
        {
            var query = Rpc.Blocks.Head.Context.Raw.DelegatesWithFrozenBalance;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/delegates_with_frozen_balance/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);

            var cycleQuery = Rpc.Blocks.Head.Context.Raw.DelegatesWithFrozenBalance[res[0]];
            Assert.Equal($"chains/main/blocks/head/context/raw/json/delegates_with_frozen_balance/{res[0]}/", cycleQuery.ToString());

            var cycleRes = await cycleQuery.GetAsync();
            Assert.True(cycleRes is DJsonArray);

            var delegateQuery = Rpc.Blocks.Head.Context.Raw.DelegatesWithFrozenBalance[res[0]][cycleRes[0]];
            Assert.Equal($"chains/main/blocks/head/context/raw/json/delegates_with_frozen_balance/{res[0]}/{cycleRes[0]}/", delegateQuery.ToString());

            var delegateRes = await delegateQuery.GetAsync();
            Assert.True(delegateRes is DJsonValue);

        }

        [Fact]
        public void TestRawContextLastBlockPriority()
        {
            //var query = Rpc.Blocks.Head.Context.Raw.LastBlockPriority;
            //Assert.Equal($"chains/main/blocks/head/context/raw/json/last_block_priority/", query.ToString());

            //var res = await query.GetAsync();
            //Assert.True(res is DJsonValue);
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
        public async Task TestRawContextRolls()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Rolls;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/rolls/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonObject);
        }

        [Fact]
        public async Task TestRawContextRollsLimbo()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Rolls.Limbo;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/rolls/limbo/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestRawContextRollsNext()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Rolls.Next;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/rolls/next/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestRawContextRollsIndex()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Rolls.Index;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/rolls/index/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);
        }

        [Fact]
        public async Task TestRawContextRollsOwnerCurrent()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Rolls.OwnerCurrent;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/rolls/owner/current/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);

        }

        [Fact]
        public async Task TestRawContextRollsOwnerSnapshot()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Rolls.OwnerSnapshot;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/rolls/owner/snapshot/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);
        }

        [Fact]
        public void TestRawContextRollsOwnerSnapshotCycleIndex()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Rolls.OwnerSnapshot[123][12]; // specific cycle and index are required
            Assert.Equal($"chains/main/blocks/head/context/raw/json/rolls/owner/snapshot/123/12/", query.ToString());

            //var res = await query.GetAsync();
            //Assert.True(res is DJsonArray);
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
        public async Task TestRawContextVotesCurrentPeriodKind()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Votes.CurrentPeriodKind;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/votes/current_period_kind/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
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
        public async Task TestRawContextVotesListingsSize()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Votes.ListingsSize;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/votes/listings_size/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
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
