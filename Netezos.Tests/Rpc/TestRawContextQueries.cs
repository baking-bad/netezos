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
        readonly int? LegacyLevel;

        public TestRawContextQueries()
        {
            var settings = DJson.Read("Rpc/settings.json");

            Rpc = new TezosRpc(settings.BaseUrl);
            TestContract = settings.TestContract;
            TestDelegate = settings.TestDelegate;
            LegacyLevel = settings.LegacyLevel;
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
            Assert.Equal($"chains/main/blocks/head/context/raw/json/active_delegates_with_rolls/{delegat}/", subQuery.ToString());

            var subRes = await subQuery.GetAsync();
            Assert.True(subRes is DJsonValue);
        }

        [Fact]
        public async Task TestRawContextCommitments()
        {
            //Add Rpc.Blocks.Head.Context.Raw.Commitments
        }

        [Fact]
        public async Task TestRawContextGlobalCounter()
        {
            //Add Rpc.Blocks.Head.Context.Raw.GlobalCounter
        }

        [Fact]
        public async Task TestRawContextContracts()
        {
            //Add Rpc.Blocks[1].Context.Raw.Contracts
        }

        [Fact]
        public async Task TestRawContextContract()
        {
            //Add Rpc.Blocks.Head.Context.Raw.Contracts[TestDelegate]
        }

        [Fact]
        public async Task TestRawContextContractBalance()
        {
            //Add Rpc.Blocks.Head.Context.Raw.Contracts[TestDelegate].Balance
        }

        [Fact]
        public async Task TestRawContextContractBigMap()
        {
            //Add Rpc.Blocks.Head.Context.Raw.Contracts[TestContract].BigMap
        }

        [Fact]
        public async Task TestRawContextContractCode()
        {
            //Add Rpc.Blocks.Head.Context.Raw.Contracts[TestContract].Code
        }

        [Fact]
        public async Task TestRawContextContractChange()
        {
            //Add Rpc.Blocks.Head.Context.Raw.Contracts[TestDelegate].Change
        }

        [Fact]
        public async Task TestRawContextContractCounter()
        {
            //Add Rpc.Blocks.Head.Context.Raw.Contracts[TestDelegate].Counter
        }

        [Fact]
        public async Task TestRawContextContractDelegatable()
        {
            //Add Rpc.Blocks.Head.Context.Raw.Contracts[TestDelegate].Delegatable
        }

        [Fact]
        public async Task TestRawContextContractDelegate()
        {
            //Add Rpc.Blocks.Head.Context.Raw.Contracts[TestDelegate].Delegate
        }

        [Fact]
        public async Task TestRawContextContractDelegateDesactivation()
        {
            //Add Rpc.Blocks.Head.Context.Raw.Contracts[TestDelegate].DelegateDesactivation
        }

        [Fact]
        public async Task TestRawContextContractInactiveDelegate()
        {
            //Add Rpc.Blocks.Head.Context.Raw.Contracts[TestDelegate].InactiveDelegate
        }

        [Fact]
        public async Task TestRawContextContractDelegated()
        {
            //Add Rpc.Blocks.Head.Context.Raw.Contracts[TestDelegate].Delegated
        }

        [Fact]
        public async Task TestRawContextContractFrozenBalance()
        {
            //Add Rpc.Blocks.Head.Context.Raw.Contracts[TestDelegate].FrozenBalance
        }

        [Fact]
        public async Task TestRawContextContractManager()
        {
            //Add Rpc.Blocks.Head.Context.Raw.Contracts[TestDelegate].Manager
        }

        [Fact]
        public async Task TestRawContextContractPaidBytes()
        {
            //Add Rpc.Blocks.Head.Context.Raw.Contracts[TestContract].PaidBytes
        }

        [Fact]
        public async Task TestRawContextContractRollList()
        {
            //Add Rpc.Blocks.Head.Context.Raw.Contracts[TestDelegate].RollList
        }

        [Fact]
        public async Task TestRawContextContractSpendable()
        {
            //Add Rpc.Blocks.Head.Context.Raw.Contracts[TestDelegate].Spendable
        }

        [Fact]
        public async Task TestRawContextContractStorage()
        {
            //Add Rpc.Blocks.Head.Context.Raw.Contracts[TestContract].Storage
        }

        [Fact]
        public async Task TestRawContextContractUsedBytes()
        {
            //Add Rpc.Blocks.Head.Context.Raw.Contracts[TestContract].UsedBytes
        }

        [Fact]
        public async Task TestRawContextCycles()
        {
            //Add Rpc.Blocks.Head.Context.Raw.Cycles
        }

        [Fact]
        public async Task TestRawContextCycle()
        {
            //Add Rpc.Blocks.Head.Context.Raw.Cycles[index]
        }

        [Fact]
        public async Task TestRawContextCycleLastRoll()
        {
            //Add Rpc.Blocks.Head.Context.Raw.Cycles[index].LastRoll
            //Add Rpc.Blocks.Head.Context.Raw.Cycles[index].LastRoll[index]
        }

        [Fact]
        public async Task TestRawContextCycleNonces()
        {
            //Add Rpc.Blocks.Head.Context.Raw.Cycles[index].Nonces
            //Add Rpc.Blocks.Head.Context.Raw.Cycles[index].Nonces[index]
        }

        [Fact]
        public async Task TestRawContextRandomSeed()
        {
            //Add Rpc.Blocks.Head.Context.Raw.Cycles[index].RandomSeed
        }

        [Fact]
        public async Task TestRawContextRollSnapshot()
        {
            //Add Rpc.Blocks.Head.Context.Raw.Cycles[index].RollSnapshot
        }

        [Fact]
        public async Task TestRawContextDelegates()
        {
            //Add Rpc.Blocks.Head.Context.Raw.Delegates
            //Add Rpc.Blocks.Head.Context.Raw.Delegates[TestDelegate]
        }

        [Fact]
        public async Task TestRawContextDelegatesWithFrozenBalance()
        {
            //Add Rpc.Blocks.Head.Context.Raw.DelegatesWithFrozenBalance
            //Add Rpc.Blocks.Head.Context.Raw.DelegatesWithFrozenBalance[cycle]
            //Add Rpc.Blocks.Head.Context.Raw.DelegatesWithFrozenBalance[cycle][delegate]
        }

        [Fact]
        public async Task TestRawContextLastBlockPriority()
        {
            //Add Rpc.Blocks.Head.Context.Raw.LastBlockPriority
        }

        [Fact]
        public async Task TestRawContextRampUp()
        {
            //Add Rpc.Blocks.Head.Context.Raw.RampUp
        }

        [Fact]
        public async Task TestRawContextRolls()
        {
            //Add Rpc.Blocks.Head.Context.Raw.Rolls
        }

        [Fact]
        public async Task TestRawContextRollsLimbo()
        {
            //Add Rpc.Blocks.Head.Context.Raw.Rolls.Limbo
        }

        [Fact]
        public async Task TestRawContextRollsNext()
        {
            //Add Rpc.Blocks.Head.Context.Raw.Rolls.Next
        }

        [Fact]
        public async Task TestRawContextRollsIndex()
        {
            //Add Rpc.Blocks.Head.Context.Raw.Rolls.Index
            //Add Rpc.Blocks.Head.Context.Raw.Rolls.Index[index]
        }

        [Fact]
        public async Task TestRawContextRollsOwnerCurrent()
        {
            //Add Rpc.Blocks.Head.Context.Raw.Rolls.OwnerCurrent
            //Add Rpc.Blocks.Head.Context.Raw.Rolls.OwnerCurrent[index]
        }

        [Fact]
        public async Task TestRawContextRollsOwnerSnapshot()
        {
            //Add Rpc.Blocks.Head.Context.Raw.Rolls.OwnerSnapshot
            //Add Rpc.Blocks.Head.Context.Raw.Rolls.OwnerSnapshot[cycle]
            //Add Rpc.Blocks.Head.Context.Raw.Rolls.OwnerSnapshot[cycle][index]
            //Add Rpc.Blocks.Head.Context.Raw.Rolls.OwnerSnapshot[cycle][index][roll]
        }

        [Fact]
        public async Task TestRawContextVotes()
        {
            //Add Rpc.Blocks.Head.Context.Raw.Votes
        }

        [Fact]
        public async Task TestRawContextVotesBallots()
        {
            //Add Rpc.Blocks.Head.Context.Raw.Votes.Ballots
        }

        [Fact]
        public async Task TestRawContextVotesCurrentPeriodKind()
        {
            //Add Rpc.Blocks.Head.Context.Raw.Votes.CurrentPeriodKind
        }

        [Fact]
        public async Task TestRawContextVotesCurrentQuorum()
        {
            //Add Rpc.Blocks.Head.Context.Raw.Votes.CurrentQuorum
        }

        [Fact]
        public async Task TestRawContextVotesListings()
        {
            //Add Rpc.Blocks.Head.Context.Raw.Votes.Listings
        }

        [Fact]
        public async Task TestRawContextVotesListingsSize()
        {
            //Add Rpc.Blocks.Head.Context.Raw.Votes.ListingsSize
        }

        [Fact]
        public async Task TestRawContextVotesProposals()
        {
            //Add Rpc.Blocks.Head.Context.Raw.Votes.Proposals
        }

        [Fact]
        public async Task TestRawContextVotesProposalsCount()
        {
            //Add Rpc.Blocks.Head.Context.Raw.Votes.ProposalsCount
        }
    }
}
