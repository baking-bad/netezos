using System;
using System.Threading.Tasks;
using Dynamic.Json;
using Netezos.Rpc;
using Xunit;

namespace Netezos.Tests.Rpc
{
    public class TestContextQueries
    {
        readonly TezosRpc Rpc;
        readonly string TestContract;
        readonly string TestDelegate;
        readonly int? LegacyLevel;

        public TestContextQueries()
        {
            var settings = DJson.Read("Rpc/settings.json");

            Rpc = new TezosRpc(settings.BaseUrl);
            TestContract = settings.TestContract;
            TestDelegate = settings.TestDelegate;
            LegacyLevel = settings.LegacyLevel;
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
        public async Task TestContextContractDelegatable()
        {
            if (LegacyLevel != null)
            {
                var query = Rpc.Blocks[Convert.ToInt32(LegacyLevel)].Context.Contracts[TestContract].Delegatable;
                Assert.Equal($"chains/main/blocks/{LegacyLevel}/context/contracts/{TestContract}/delegatable/", query.ToString());

                var res = await query.GetAsync();
                Assert.True(res is DJsonValue);
            }
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
        public async Task TestContextContractManager()
        {
            if (LegacyLevel != null)
            {
                var query = Rpc.Blocks[Convert.ToInt32(LegacyLevel)].Context.Contracts[TestDelegate].Manager;
                Assert.Equal($"chains/main/blocks/{LegacyLevel}/context/contracts/{TestDelegate}/manager/", query.ToString());

                var res = await query.GetAsync();
                Assert.True(res is DJsonValue);
            }
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
        public async Task TestContextContractSpendable()
        {
            if (LegacyLevel != null)
            {
                var query = Rpc.Blocks[Convert.ToInt32(LegacyLevel)].Context.Contracts[TestDelegate].Spendable;
                Assert.Equal($"chains/main/blocks/{LegacyLevel}/context/contracts/{TestDelegate}/spendable/", query.ToString());

                var res = await query.GetAsync();
                Assert.True(res is DJsonValue);
            }
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
            //Also test GetAsync(DelegateStatus.Active) and GetAsync(DelegateStatus.Inactive)
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
            var head = await Rpc.Blocks.Head.Header.GetAsync();
            int level = head.level;
            var query = Rpc.Blocks.Head.Context.Nonces[level];
            Assert.Equal($"chains/main/blocks/head/context/nonces/{level}/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonObject);
        }

        [Fact]
        public async Task TestContextSeed()
        {
            var query = Rpc.Blocks.Head.Context.Seed;
            Assert.Equal($"chains/main/blocks/head/context/seed/", query.ToString());

            var res = await query.PostAsync(query);
            Assert.True(res is DJsonValue);
        }
    }
}
