using System;
using System.Threading.Tasks;
using Netezos.Rpc;
using Xunit;

namespace Netezos.Tests.Rpc.TestQueries
{
    public class TestBlockQuery
    {
        readonly TezosRpc Rpc = new TezosRpc("https://mainnet-tezos.giganode.io/");

        [Fact]
        public async Task TestBlockHash()
        {
            string hash = await Rpc.Blocks.Head.Hash.GetAsync();

            Assert.NotNull(hash);
            Assert.True(hash.Length == 51);
        }

        [Fact]
        public async Task TestBlockHeader()
        {
            var header = await Rpc.Blocks.Head.Header.GetAsync();

            Assert.NotNull(header);
        }

        [Fact]
        public async Task TestBlockHeaderProtocolData()
        {
            var protocolData = await Rpc.Blocks.Head.Header.ProtocolData.GetAsync();

            Assert.NotNull(protocolData);
        }

        [Fact]
        public async Task TestBlockHeaderPdRaw()
        {
            var pdRaw = await Rpc.Blocks.Head.Header.ProtocolData.Raw.GetAsync();

            Assert.NotNull(pdRaw);
        }

        [Fact]
        public async Task TestBlockHeaderRaw()
        {
            var headerRaw = await Rpc.Blocks.Head.Header.Raw.GetAsync();

            Assert.NotNull(headerRaw);
        }

        [Fact]
        public async Task TestBlockHeaderShell()
        {
            var headerShell = await Rpc.Blocks.Head.Header.Shell.GetAsync();

            Assert.NotNull(headerShell);
        }

        [Fact]
        public async Task TestBlockMetadata()
        {
            var metaData = await Rpc.Blocks.Head.Metadata.GetAsync();

            Assert.NotNull(metaData);
        }

        [Fact]
        public async Task TestLiveBlocks()
        {
            var liveBlocks = await Rpc.Blocks.Head.LiveBlocks.GetAsync();

            Assert.NotNull(liveBlocks);
            Assert.True(liveBlocks.Count >= 0);
        }

        [Fact]
        public async Task TestBlockContextConstants()
        {
            var constants = await Rpc.Blocks.Head.Context.Constants.GetAsync();

            Assert.NotNull(constants);
        }

        [Fact]
        public async Task TestBlockContextConstantsErrors()
        {
            var errors = await Rpc.Blocks.Head.Context.Constants.Errors.GetAsync();

            Assert.NotNull(errors);
        }

        [Fact]
        public async Task TestBlockContextDelegates()
        {
            var delegates = await Rpc.Blocks.Head.Context.Delegates.GetAsync();

            Assert.NotNull(delegates);
            Assert.True(delegates.Count >= 0);
        }

        [Fact]
        public async Task TestBlockContextRaw()
        {
            var contextRaw = await Rpc.Blocks.Head.Context.Raw.GetAsync();

            Assert.NotNull(contextRaw);
        }

        [Fact]
        public async Task TestBlockHelpersBakingRights()
        {
            var BakingRights = await Rpc.Blocks.Head.Helpers.BakingRights.GetAsync();

            Assert.NotNull(BakingRights);
            Assert.True(BakingRights.Count >= 0);
        }

        [Fact]
        public async Task TestBlockHelpersEndorsingRights()
        {
            var endorsingRights = await Rpc.Blocks.Head.Helpers.EndorsingRights.GetAsync();

            Assert.NotNull(endorsingRights);
            Assert.True(endorsingRights.Count >= 0);
        }

        [Fact]
        public async Task TestBlockOperationHashaes()
        {
            var OperationHashes = await Rpc.Blocks.Head.OperationsHashes.GetAsync();

            Assert.NotNull(OperationHashes);
            Assert.True(OperationHashes.Count >= 0);
        }

        [Fact]
        public async Task TestBlockOperations()
        {
            var operation = await Rpc.Blocks.Head.Operations.GetAsync();

            Assert.NotNull(operation);
            Assert.True(operation.Count >= 0);
        }

        [Fact]
        public async Task TestBlockContractsBalaance()
        {
            var balance = await Rpc.Blocks.Head.Context.Contracts["KT1VG2WtYdSWz5E7chTeAdDPZNy2MpP8pTfL"].Balance.GetAsync();

            Assert.NotNull(balance);
        }

        [Fact]
        public async Task TestBlockContextContractID()
        {
            var contractsID = await Rpc.Blocks.Head.Context.Contracts["tz1WBfwbT66FC6BTLexc2BoyCCBM9LG7pnVW"].GetAsync();

            Assert.NotNull(contractsID);
        }

        [Fact]
        public async Task TestBlockContractsDelegate()
        {
            var contractsDelegate = await Rpc.Blocks.Head.Context.Contracts["tz1WnfXMPaNTBmH7DBPwqCWs9cPDJdkGBTZ8"].Delegate.GetAsync();

            Assert.NotNull(contractsDelegate);
        }

        [Fact]
        public async Task TestBlockContractsCounter()
        {
            var counter = await Rpc.Blocks.Head.Context.Contracts["tz1WnfXMPaNTBmH7DBPwqCWs9cPDJdkGBTZ8"].Counter.GetAsync();

            Assert.NotNull(counter);
        }

        [Fact]
        public async Task TestBlockContractsManagerKey()
        {
            var managerKey = await Rpc.Blocks.Head.Context.Contracts["tz1WnfXMPaNTBmH7DBPwqCWs9cPDJdkGBTZ8"].ManagerKey.GetAsync();

            Assert.NotNull(managerKey);
        }

        [Fact]
        public async Task TestBlockContractsStorage()
        {
            var storage = await Rpc.Blocks.Head.Context.Contracts["KT1VG2WtYdSWz5E7chTeAdDPZNy2MpP8pTfL"].Storage.GetAsync();

            Assert.NotNull(storage);
        }

        [Fact]
        public async Task TestBlockContractsScript()
        {
            var script = await Rpc.Blocks.Head.Context.Contracts["KT1VG2WtYdSWz5E7chTeAdDPZNy2MpP8pTfL"].Script.GetAsync();

            Assert.NotNull(script);
        }

        [Fact]
        public async Task TestBlockVotesBallotList()
        {
            var balootList = await Rpc.Blocks.Head.Votes.BallotList.GetAsync();

            Assert.NotNull(balootList);
            Assert.True(balootList.Count >= 0);
        }

        [Fact]
        public async Task TestBlockVotesBallot()
        {
            var ballot = await Rpc.Blocks.Head.Votes.Ballots.GetAsync();

            Assert.NotNull(ballot);
        }

        [Fact]
        public async Task TestBlockVotesCurrentPK()
        {
            var currentPK = await Rpc.Blocks.Head.Votes.CurrentPeriodKind.GetAsync();

            Assert.NotNull(currentPK);
        }

        [Fact]
        public async Task TestBlockVotesCurrentProposal()
        {
            var currentProposal = await Rpc.Blocks.Head.Votes.CurrentProposals.GetAsync();

            Assert.NotNull(currentProposal);
        }

        [Fact]
        public async Task TestBlockVotesCurrentQuorum()
        {
            var currentQuorum = await Rpc.Blocks.Head.Votes.CurrentQuorum.GetAsync();

            Assert.NotNull(currentQuorum);
        }

        [Fact]
        public async Task TestBlockVotesListings()
        {
            var listings = await Rpc.Blocks.Head.Votes.Listings.GetAsync();

            Assert.NotNull(listings);
            Assert.True(listings.Count >= 0);
        }

        [Fact]
        public async Task TestBlockVotesProposals()
        {
            var proposals = await Rpc.Blocks.Head.Votes.Proposals.GetAsync();

            Assert.NotNull(proposals);
            Assert.True(proposals.Count >= 0);
        }

        [Fact]
        public async Task TestBlockDelegates()
        {
            var delegates = await Rpc.Blocks.Head.Context.Delegates["tz1WnfXMPaNTBmH7DBPwqCWs9cPDJdkGBTZ8"].GetAsync();

            Assert.NotNull(delegates);
        }

        [Fact]
        public async Task TestBlockDelegatesBalance()
        {
            var balance = await Rpc.Blocks.Head.Context.Delegates["tz1WnfXMPaNTBmH7DBPwqCWs9cPDJdkGBTZ8"].Balance.GetAsync();

            Assert.NotNull(balance);
        }

        [Fact]
        public async Task TestBlockDelegatesDeactivated()
        {
            var deactivated = await Rpc.Blocks.Head.Context.Delegates["tz1WnfXMPaNTBmH7DBPwqCWs9cPDJdkGBTZ8"].Deactivated.GetAsync();

            Assert.NotNull(deactivated);
        }

        [Fact]
        public async Task TestBlockDelegatesDelegatedBalance()
        {
            var delegatedBalance = await Rpc.Blocks.Head.Context.Delegates["tz1WnfXMPaNTBmH7DBPwqCWs9cPDJdkGBTZ8"].DelegatedBalance.GetAsync();

            Assert.NotNull(delegatedBalance);
        }

        [Fact]
        public async Task TestBlockDelegatesDelegatedContracts()
        {
            var delegatedContracts = await Rpc.Blocks.Head.Context.Delegates["tz1WnfXMPaNTBmH7DBPwqCWs9cPDJdkGBTZ8"].DelegatedContracts.GetAsync();

            Assert.NotNull(delegatedContracts);
            Assert.True(delegatedContracts.Count >= 0);
        }

        [Fact]
        public async Task TestBlockDelegatesFrozenBalance()
        {
            var frozenBalance = await Rpc.Blocks.Head.Context.Delegates["tz1WnfXMPaNTBmH7DBPwqCWs9cPDJdkGBTZ8"].FrozenBalance.GetAsync();

            Assert.NotNull(frozenBalance);
        }

        [Fact]
        public async Task TestBlockDelegatesFrozenBalanceBC()
        {
            var frozenBalanceBC = await Rpc.Blocks.Head.Context.Delegates["tz1WnfXMPaNTBmH7DBPwqCWs9cPDJdkGBTZ8"].FrozenBalanceByCycle.GetAsync();

            Assert.NotNull(frozenBalanceBC);
            Assert.True(frozenBalanceBC.Count >= 0);
        }


        [Fact]
        public async Task TestBlockDelegatesGracePeriod()
        {
            var gracePeriod = await Rpc.Blocks.Head.Context.Delegates["tz1WnfXMPaNTBmH7DBPwqCWs9cPDJdkGBTZ8"].GracePeriod.GetAsync();

            Assert.NotNull(gracePeriod);
        }

        [Fact]
        public async Task TestBlockDelegatesStakingBalance()
        {
            var stakingBalance = await Rpc.Blocks.Head.Context.Delegates["tz1WnfXMPaNTBmH7DBPwqCWs9cPDJdkGBTZ8"].StakingBalance.GetAsync();

            Assert.NotNull(stakingBalance);
        }   
    }
}
