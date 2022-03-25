using System.Threading.Tasks;
using Dynamic.Json;
using Netezos.Rpc;
using Netezos.Rpc.Queries.Post;
using Xunit;

namespace Netezos.Tests.Rpc
{
    public class TestContextQueries : IClassFixture<SettingsFixture>
    {
        readonly TezosRpc Rpc;
        readonly string TestContract;
        readonly string TestEntrypoint;
        readonly string TestDelegate;
        readonly string KeyHash;
        readonly int BigMapId;

        public TestContextQueries(SettingsFixture settings)
        {
            Rpc = settings.Rpc;
            TestContract = settings.TestContract;
            TestEntrypoint = settings.TestEntrypoint;
            TestDelegate = settings.TestDelegate;
            KeyHash = settings.KeyHash;
            BigMapId = settings.BigMapId;
        }

        [Fact]
        public async Task TestContextBigMaps()
        {
            var query = Rpc.Blocks.Head.Context.BigMaps[BigMapId][KeyHash];
            Assert.Equal($"chains/main/blocks/head/context/big_maps/{BigMapId}/{KeyHash}/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonObject);
        }
        
        [Fact]
        public async Task TestContextBigMapsNormalized()
        {
            var query = Rpc.Blocks.Head.Context.BigMaps[BigMapId][KeyHash].Normalized;
            Assert.Equal($"chains/main/blocks/head/context/big_maps/{BigMapId}/{KeyHash}/normalized/", query.ToString());

            var res = await query.PostAsync(NormalizedQuery.UnparsingMode.Readable);
            Assert.True(res is DJsonObject);
        }

        [Fact]
        public async Task TestContextConstants()
        {
            var query = Rpc.Blocks.Head.Context.Constants;
            Assert.Equal("chains/main/blocks/head/context/constants/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonObject);
        }

        [Fact]
        public async Task TestContextConstantsErrors()
        {
            var query = Rpc.Blocks.Head.Context.Constants.Errors;
            Assert.Equal("chains/main/blocks/head/context/constants/errors", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonObject);
        }

        [Fact]
        public async Task TestContextContract()
        {
            var query = Rpc.Blocks.Head.Context.Contracts[TestContract];
            Assert.Equal($"chains/main/blocks/head/context/contracts/{TestContract}/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonObject);
        }

        [Fact]
        public async Task TestContextContractBalance()
        {
            var query = Rpc.Blocks.Head.Context.Contracts[TestContract].Balance;
            Assert.Equal($"chains/main/blocks/head/context/contracts/{TestContract}/balance/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestContextContractCounter()
        {
            var query = Rpc.Blocks.Head.Context.Contracts[TestDelegate].Counter;
            Assert.Equal($"chains/main/blocks/head/context/contracts/{TestDelegate}/counter/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestContextContractDelegate()
        {
            var query = Rpc.Blocks.Head.Context.Contracts[TestContract].Delegate;
            Assert.Equal($"chains/main/blocks/head/context/contracts/{TestContract}/delegate/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestContextContractEntrypoints()
        {
            var query = Rpc.Blocks.Head.Context.Contracts[TestContract].Entrypoints;
            Assert.Equal($"chains/main/blocks/head/context/contracts/{TestContract}/entrypoints/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonObject);
        }

        [Fact]
        public async Task TestContextContractEntrypoint()
        {
            var query = Rpc.Blocks.Head.Context.Contracts[TestContract].Entrypoints[TestEntrypoint];
            Assert.Equal($"chains/main/blocks/head/context/contracts/{TestContract}/entrypoints/{TestEntrypoint}/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonObject);
        }

        [Fact]
        public async Task TestContextContractManagerKey()
        {
            var query = Rpc.Blocks.Head.Context.Contracts[TestDelegate].ManagerKey;
            Assert.Equal($"chains/main/blocks/head/context/contracts/{TestDelegate}/manager_key/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestContextContractScript()
        {
            var query = Rpc.Blocks.Head.Context.Contracts[TestContract].Script;
            Assert.Equal($"chains/main/blocks/head/context/contracts/{TestContract}/script/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonObject);
        }

        [Fact]
        public async Task TestContextContractScriptNormalized()
        {
            var query = Rpc.Blocks.Head.Context.Contracts[TestContract].Script.Normalized;
            Assert.Equal($"chains/main/blocks/head/context/contracts/{TestContract}/script/normalized/", query.ToString());

            var res = await query.PostAsync(NormalizedQuery.UnparsingMode.Readable);
            Assert.True(res is DJsonObject);
        }

        [Fact]
        public async Task TestContextContractStorage()
        {
            var query = Rpc.Blocks.Head.Context.Contracts[TestContract].Storage;
            Assert.Equal($"chains/main/blocks/head/context/contracts/{TestContract}/storage/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonObject);
        }

        [Fact]
        public async Task TestContextContractStorageNormalized()
        {
            var query = Rpc.Blocks.Head.Context.Contracts[TestContract].Storage.Normalized;
            Assert.Equal($"chains/main/blocks/head/context/contracts/{TestContract}/storage/normalized/", query.ToString());

            var res = await query.PostAsync(NormalizedQuery.UnparsingMode.Readable);
            Assert.True(res is DJsonObject);
        }

        [Fact]
        public async Task TestContextDelegates()
        {
            var query = Rpc.Blocks.Head.Context.Delegates;
            Assert.Equal($"chains/main/blocks/head/context/delegates/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);

            var deleegateActiveRes = await query.GetAsync(DelegateStatus.Active);
            Assert.True(deleegateActiveRes is DJsonArray);

            var delegateInactiveRes = await query.GetAsync(DelegateStatus.Inactive);
            Assert.True(delegateInactiveRes is DJsonArray);
        }

        [Fact]
        public async Task TestContextDelegate()
        {
            var query = Rpc.Blocks.Head.Context.Delegates[TestDelegate];
            Assert.Equal($"chains/main/blocks/head/context/delegates/{TestDelegate}/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonObject);
        }

        [Fact]
        public async Task TestContextDelegateCurrentFrozenDeposits()
        {
            var query = Rpc.Blocks.Head.Context.Delegates[TestDelegate].CurrentFrozenDeposits;
            Assert.Equal($"chains/main/blocks/head/context/delegates/{TestDelegate}/current_frozen_deposits/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestContextDelegateDeactivated()
        {
            var query = Rpc.Blocks.Head.Context.Delegates[TestDelegate].Deactivated;
            Assert.Equal($"chains/main/blocks/head/context/delegates/{TestDelegate}/deactivated/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestContextDelegateDelegatedBalance()
        {
            var query = Rpc.Blocks.Head.Context.Delegates[TestDelegate].DelegatedBalance;
            Assert.Equal($"chains/main/blocks/head/context/delegates/{TestDelegate}/delegated_balance/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestContextDelegateDelegatedContracts()
        {
            var query = Rpc.Blocks.Head.Context.Delegates[TestDelegate].DelegatedContracts;
            Assert.Equal($"chains/main/blocks/head/context/delegates/{TestDelegate}/delegated_contracts/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);
        }
        
        [Fact]
        public async Task TestContextDelegateFrozenDeposits()
        {
            var query = Rpc.Blocks.Head.Context.Delegates[TestDelegate].FrozenDeposits;
            Assert.Equal($"chains/main/blocks/head/context/delegates/{TestDelegate}/frozen_deposits/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestContextDelegateFrozenDepositsLimit()
        {
            var query = Rpc.Blocks.Head.Context.Delegates[TestDelegate].FrozenDepositsLimit;
            Assert.Equal($"chains/main/blocks/head/context/delegates/{TestDelegate}/frozen_deposits_limit/", query.ToString());

            var res = await query.GetAsync();
            if (res != null)
                Assert.True(res is DJsonValue);
        }
        
        [Fact]
        public async Task TestContextDelegateFullBalance()
        {
            var query = Rpc.Blocks.Head.Context.Delegates[TestDelegate].FullBalance;
            Assert.Equal($"chains/main/blocks/head/context/delegates/{TestDelegate}/full_balance/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestContextDelegateGracePeriod()
        {
            var query = Rpc.Blocks.Head.Context.Delegates[TestDelegate].GracePeriod;
            Assert.Equal($"chains/main/blocks/head/context/delegates/{TestDelegate}/grace_period/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestContextDelegatParticipation()
        {
            var query = Rpc.Blocks.Head.Context.Delegates[TestDelegate].Participation;
            Assert.Equal($"chains/main/blocks/head/context/delegates/{TestDelegate}/participation/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonObject);
        }

        [Fact]
        public async Task TestContextDelegateStakingBalance()
        {
            var query = Rpc.Blocks.Head.Context.Delegates[TestDelegate].StakingBalance;
            Assert.Equal($"chains/main/blocks/head/context/delegates/{TestDelegate}/staking_balance/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestContextDelegateVotingPower()
        {
            var query = Rpc.Blocks.Head.Context.Delegates[TestDelegate].VotingPower;
            Assert.Equal($"chains/main/blocks/head/context/delegates/{TestDelegate}/voting_power/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestContextNonces()
        {
            // Returns 404 for the SmartPy Node
            var query = Rpc.Blocks.Head.Context.Nonces[1234]; // specific block level is required
            Assert.Equal($"chains/main/blocks/head/context/nonces/{1234}", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonObject);
        }

        [Fact]
        public async Task TestContextSeed()
        {
            // Returns 401 for Giganode
            // Returns 404 for the SmartPy Node
            var query = Rpc.Blocks.Head.Context.Seed; // this is a POST request
            Assert.Equal($"chains/main/blocks/head/context/seed", query.ToString());

            var res = await query.PostAsync(query);
            Assert.True(res is DJsonValue);
        }
    }
}
