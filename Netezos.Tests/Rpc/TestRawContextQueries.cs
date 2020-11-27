using System;
using System.Threading.Tasks;
using Dynamic.Json;
using Netezos.Rpc;
using Xunit;

namespace Netezos.Tests.Rpc
{
    public class TestRawContextQueries
    {
        readonly TezosRpc Rpc;
        readonly string TestContract;
        readonly string TestDelegate;
        readonly string InactiveDelegate;
        readonly int? LegacyLevel;
        readonly int index;
        readonly int RollsIndex;
        readonly int cycle;

        public TestRawContextQueries()
        {
            var settings = DJson.Read("Rpc/settings.json");

            Rpc = new TezosRpc(settings.BaseUrl, settings.timeout);
            TestContract = settings.TestContract;
            TestDelegate = settings.TestDelegate;
            InactiveDelegate = settings.InactiveDelegate;
            LegacyLevel = settings.LegacyLevel;
            index = settings.index;
            RollsIndex = settings.RollsIndex;
            cycle = settings.cycle;
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

            var delegat = (string)res[0];

            var subQuery = Rpc.Blocks.Head.Context.Raw.ActiveDelegates[delegat];
            Assert.Equal($"chains/main/blocks/head/context/raw/json/active_delegates_with_rolls/{delegat}/",
                subQuery.ToString());

            var subRes = await subQuery.GetAsync();
            Assert.True(subRes is DJsonValue);
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
            Assert.Equal($"chains/main/blocks/head/context/raw/json/contracts/index/{TestDelegate}/balance/",
                query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestRawContextContractBigMap()
        {
            var query = Rpc.Blocks[Convert.ToInt32(LegacyLevel)].Context.Raw.Contracts[TestContract].BigMap;
            Assert.Equal($"chains/main/blocks/{LegacyLevel}/context/raw/json/contracts/index/{TestContract}/big_map/",
                query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);
        }

        [Fact]
        public async Task TestRawContextContractCode()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Contracts[TestContract].Code;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/contracts/index/{TestContract}/code/", 
                query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);
        }

        [Fact]
        public async Task TestRawContextContractChange()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Contracts[TestDelegate].Change;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/contracts/index/{TestDelegate}/change/",
                query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestRawContextContractCounter()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Contracts[TestDelegate].Counter;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/contracts/index/{TestDelegate}/counter/",
                query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestRawContextContractDelegatable()
        {
            string Contract = "KT1WJ1tKARGLmEhrUyLyTUjXdBfaEQQjyvkZ";
            var query = Rpc.Blocks[Convert.ToInt32(LegacyLevel)].Context.Raw.Contracts[Contract].Delegatable;
            Assert.Equal($"chains/main/blocks/{LegacyLevel}/context/raw/json/contracts/index/{Contract}/delegatable/", 
                query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestRawContextContractDelegate()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Contracts[TestDelegate].Delegate;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/contracts/index/{TestDelegate}/delegate/", 
                query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestRawContextContractDelegateDesactivation()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Contracts[TestDelegate].DelegateDesactivation;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/contracts/index/{TestDelegate}/delegate_desactivation/",
                query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestRawContextContractInactiveDelegate()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Contracts[InactiveDelegate].InactiveDelegate;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/contracts/index/{InactiveDelegate}/inactive_delegate/",
                query.ToString());


           var res = await query.GetAsync();
           Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestRawContextContractDelegated()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Contracts[TestDelegate].Delegated;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/contracts/index/{TestDelegate}/delegated/",
                query.ToString());

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
            Assert.Equal($"chains/main/blocks/head/context/raw/json/contracts/index/{TestDelegate}/roll_list/",
                query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestRawContextContractSpendable()
        {
            var Contract = "KT1WJ1tKARGLmEhrUyLyTUjXdBfaEQQjyvkZ";
            var query = Rpc.Blocks[Convert.ToInt32(LegacyLevel)].Context.Raw.Contracts[Contract].Spendable;
            Assert.Equal($"chains/main/blocks/{LegacyLevel}/context/raw/json/contracts/index/{Contract}/spendable/",
                query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestRawContextContractStorage()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Contracts[TestContract].Storage;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/contracts/index/{TestContract}/storage/",
                query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonObject);
        }

        [Fact]
        public async Task TestRawContextContractUsedBytes()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Contracts[TestContract].UsedBytes;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/contracts/index/{TestContract}/used_bytes/",
                query.ToString());

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
        public async Task TestRawContextCycle()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Cycles[cycle];
            Assert.Equal($"chains/main/blocks/head/context/raw/json/cycle/{cycle}/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonObject);
        }

        [Fact]
        public async Task TestRawContextCycleLastRoll()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Cycles[cycle].LastRoll;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/cycle/{cycle}/last_roll/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);
        }

        [Fact]
        public async Task TestRawContextCycleNonces()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Cycles[cycle].Nonces;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/cycle/{cycle}/nonces/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);
        }

        [Fact]
        public async Task TestRawContextRandomSeed()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Cycles[cycle].RandomSeed;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/cycle/{cycle}/random_seed/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestRawContextRollSnapshot()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Cycles[cycle].RollSnapshot;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/cycle/{cycle}/roll_snapshot/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestRawContextDelegates()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Delegates;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/delegates/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);

            var subQuery = Rpc.Blocks.Head.Context.Raw.Delegates[TestDelegate];
            Assert.Equal($"chains/main/blocks/head/context/raw/json/delegates/{TestDelegate}/", subQuery.ToString());

            var subRes = await subQuery.GetAsync();
            Assert.True(subRes is DJsonValue);
        }

        [Fact]
        public async Task TestRawContextDelegatesWithFrozenBalance()
        {
            var query = Rpc.Blocks.Head.Context.Raw.DelegatesWithFrozenBalance;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/delegates_with_frozen_balance/",
                query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);

            var cycle = (string)res[0];
            var cycleQuery = Rpc.Blocks.Head.Context.Raw.DelegatesWithFrozenBalance[Convert.ToInt32(cycle)];
            Assert.Equal($"chains/main/blocks/head/context/raw/json/delegates_with_frozen_balance/{cycle}/",
                cycleQuery.ToString());

            var cycleRes = await cycleQuery.GetAsync();
            Assert.True(cycleRes is DJsonArray);

            var delegat = (string)cycleRes[0];
            var delegatQuery = Rpc.Blocks.Head.Context.Raw.DelegatesWithFrozenBalance[Convert.ToInt32(cycle)][delegat];
            Assert.Equal($"chains/main/blocks/head/context/raw/json/delegates_with_frozen_balance/{cycle}/{delegat}/", 
                delegatQuery.ToString());

            var delegatRes = await delegatQuery.GetAsync();
            Assert.True(delegatRes is DJsonValue);

        }

        [Fact]
        public async Task TestRawContextLastBlockPriority()
        {
            var query = Rpc.Blocks[Convert.ToInt32(LegacyLevel)].Context.Raw.LastBlockPriority;
            Assert.Equal($"chains/main/blocks/{LegacyLevel}/context/raw/json/last_block_priority/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
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

            var indexRes = await Rpc.Blocks.Head.Context.Raw.Rolls.OwnerCurrent[RollsIndex].GetAsync();
            Assert.True(indexRes is DJsonValue);

        }

        [Fact]
        public async Task TestRawContextRollsOwnerSnapshot()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Rolls.OwnerSnapshot;
            Assert.Equal($"chains/main/blocks/head/context/raw/json/rolls/owner/snapshot/",
                query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);
        }

        [Fact]
        public async Task TestRawContextRollsOwnerSnapshotCycleIndex()
        {
            var query = Rpc.Blocks.Head.Context.Raw.Rolls.OwnerSnapshot[cycle][index];
            Assert.Equal($"chains/main/blocks/head/context/raw/json/rolls/owner/snapshot/{cycle}/{index}/",
                query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);
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
        public async Task TestRawContextVotesCurrentQuorum()
        {
            var query = Rpc.Blocks[Convert.ToInt32(LegacyLevel)].Context.Raw.Votes.CurrentQuorum;
            Assert.Equal($"chains/main/blocks/{LegacyLevel}/context/raw/json/votes/current_quorum/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
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
