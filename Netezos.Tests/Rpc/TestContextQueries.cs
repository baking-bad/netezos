using System.Threading.Tasks;
using Dynamic.Json;
using Netezos.Rpc;
using Xunit;

namespace Netezos.Tests.Rpc
{
    public class TestContextQueries : IClassFixture<SettingsFixture>
    {
        readonly TezosRpc Rpc;
        readonly string TestContract;
        readonly string TestDelegate;

        public TestContextQueries(SettingsFixture settings)
        {
            Rpc = settings.Rpc;
            TestContract = settings.TestContract;
            TestDelegate = settings.TestDelegate;
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
            Assert.Equal("chains/main/blocks/head/context/constants/errors/", query.ToString());

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
        public void TestContextContractDelegatable()
        {
            //var query = Rpc.Blocks.Head.Context.Contracts[TestContract].Delegatable;
            //Assert.Equal($"chains/main/blocks/head/context/contracts/{TestContract}/delegatable/", query.ToString());

            //var res = await query.GetAsync();
            //Assert.True(res is DJsonValue);
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
        public void TestContextContractManager()
        {
            //var query = Rpc.Blocks.Head.Context.Contracts[TestDelegate].Manager;
            //Assert.Equal($"chains/main/blocks/head/context/contracts/{TestDelegate}/manager/", query.ToString());

            //var res = await query.GetAsync();
            //Assert.True(res is DJsonValue);
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
        public void TestContextContractSpendable()
        {
            //var query = Rpc.Blocks.Head.Context.Contracts[TestDelegate].Spendable;
            //Assert.Equal($"chains/main/blocks/head/context/contracts/{TestDelegate}/spendable/", query.ToString());

            //var res = await query.GetAsync();
            //Assert.True(res is DJsonValue);
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
        public async Task TestContextDelegateBalance()
        {
            var query = Rpc.Blocks.Head.Context.Delegates[TestDelegate].Balance;
            Assert.Equal($"chains/main/blocks/head/context/delegates/{TestDelegate}/balance/", query.ToString());

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
        public async Task TestContextDelegateFrozenBalance()
        {
            var query = Rpc.Blocks.Head.Context.Delegates[TestDelegate].FrozenBalance;
            Assert.Equal($"chains/main/blocks/head/context/delegates/{TestDelegate}/frozen_balance/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestContextDelegateFrozenBalanceByCycle()
        {
            var query = Rpc.Blocks.Head.Context.Delegates[TestDelegate].FrozenBalanceByCycle;
            Assert.Equal($"chains/main/blocks/head/context/delegates/{TestDelegate}/frozen_balance_by_cycle/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);
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
        public async Task TestContextDelegateStakingBalance()
        {
            var query = Rpc.Blocks.Head.Context.Delegates[TestDelegate].StakingBalance;
            Assert.Equal($"chains/main/blocks/head/context/delegates/{TestDelegate}/staking_balance/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestContextNonces()
        {
            var query = Rpc.Blocks.Head.Context.Nonces[1234]; // specific block level is required
            Assert.Equal($"chains/main/blocks/head/context/nonces/{1234}/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonObject);
        }

        [Fact]
        public async Task TestContextSeed()
        {
            var query = Rpc.Blocks.Head.Context.Seed; // this is a POST request
            Assert.Equal($"chains/main/blocks/head/context/seed/", query.ToString());

            var res = await query.PostAsync(query);
            Assert.True(res is DJsonValue);
        }
    }
}
