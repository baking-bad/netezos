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
                //Add test for Rpc.Blocks[LegacyLevel].Context.Contracts[DelegateAddress].Delegatable
            }
        }

        [Fact]
        public async Task TestContextContractDelegate()
        {
            var contractsDelegate = await Rpc.Blocks.Head.Context.Contracts["tz1WnfXMPaNTBmH7DBPwqCWs9cPDJdkGBTZ8"].Delegate.GetAsync();

            Assert.NotNull(contractsDelegate);
        }

        [Fact]
        public async Task TestContextContractManager()
        {
            if (LegacyLevel != null)
            {
                //Add test for Rpc.Blocks[LegacyLevel].Context.Contracts[DelegateAddress].Manager
            }
        }

        [Fact]
        public async Task TestContextContractManagerKey()
        {
            var managerKey = await Rpc.Blocks.Head.Context.Contracts["tz1WnfXMPaNTBmH7DBPwqCWs9cPDJdkGBTZ8"].ManagerKey.GetAsync();

            Assert.NotNull(managerKey);
        }

        [Fact]
        public async Task TestContextContractScript()
        {
            var script = await Rpc.Blocks.Head.Context.Contracts["KT1VG2WtYdSWz5E7chTeAdDPZNy2MpP8pTfL"].Script.GetAsync();

            Assert.NotNull(script);
        }

        [Fact]
        public async Task TestContextContractSpendable()
        {
            if (LegacyLevel != null)
            {
                //Add test for Rpc.Blocks[LegacyLevel].Context.Contracts[DelegateAddress].Spendable
            }
        }

        [Fact]
        public async Task TestContextContractStorage()
        {
            var storage = await Rpc.Blocks.Head.Context.Contracts["KT1VG2WtYdSWz5E7chTeAdDPZNy2MpP8pTfL"].Storage.GetAsync();

            Assert.NotNull(storage);
        }

        [Fact]
        public async Task TestContextDelegates()
        {
            var delegates = await Rpc.Blocks.Head.Context.Delegates.GetAsync();

            Assert.NotNull(delegates);
            Assert.True(delegates.Count >= 0);

            //Also test GetAsync(DelegateStatus.Active) and GetAsync(DelegateStatus.Inactive)
        }

        [Fact]
        public async Task TestContextDelegate()
        {
            var delegates = await Rpc.Blocks.Head.Context.Delegates["tz1WnfXMPaNTBmH7DBPwqCWs9cPDJdkGBTZ8"].GetAsync();

            Assert.NotNull(delegates);
        }

        [Fact]
        public async Task TestContextDelegateBalance()
        {
            var balance = await Rpc.Blocks.Head.Context.Delegates["tz1WnfXMPaNTBmH7DBPwqCWs9cPDJdkGBTZ8"].Balance.GetAsync();

            Assert.NotNull(balance);
        }

        [Fact]
        public async Task TestContextDelegateDeactivated()
        {
            var deactivated = await Rpc.Blocks.Head.Context.Delegates["tz1WnfXMPaNTBmH7DBPwqCWs9cPDJdkGBTZ8"].Deactivated.GetAsync();

            Assert.NotNull(deactivated);
        }

        [Fact]
        public async Task TestContextDelegateDelegatedBalance()
        {
            var delegatedBalance = await Rpc.Blocks.Head.Context.Delegates["tz1WnfXMPaNTBmH7DBPwqCWs9cPDJdkGBTZ8"].DelegatedBalance.GetAsync();

            Assert.NotNull(delegatedBalance);
        }

        [Fact]
        public async Task TestContextDelegateDelegatedContracts()
        {
            var delegatedContracts = await Rpc.Blocks.Head.Context.Delegates["tz1WnfXMPaNTBmH7DBPwqCWs9cPDJdkGBTZ8"].DelegatedContracts.GetAsync();

            Assert.NotNull(delegatedContracts);
            Assert.True(delegatedContracts.Count >= 0);
        }

        [Fact]
        public async Task TestContextDelegateFrozenBalance()
        {
            var frozenBalance = await Rpc.Blocks.Head.Context.Delegates["tz1WnfXMPaNTBmH7DBPwqCWs9cPDJdkGBTZ8"].FrozenBalance.GetAsync();

            Assert.NotNull(frozenBalance);
        }

        [Fact]
        public async Task TestContextDelegateFrozenBalanceByCycle()
        {
            var frozenBalanceBC = await Rpc.Blocks.Head.Context.Delegates["tz1WnfXMPaNTBmH7DBPwqCWs9cPDJdkGBTZ8"].FrozenBalanceByCycle.GetAsync();

            Assert.NotNull(frozenBalanceBC);
            Assert.True(frozenBalanceBC.Count >= 0);
        }

        [Fact]
        public async Task TestContextDelegateGracePeriod()
        {
            var gracePeriod = await Rpc.Blocks.Head.Context.Delegates["tz1WnfXMPaNTBmH7DBPwqCWs9cPDJdkGBTZ8"].GracePeriod.GetAsync();

            Assert.NotNull(gracePeriod);
        }

        [Fact]
        public async Task TestContextDelegateStakingBalance()
        {
            var stakingBalance = await Rpc.Blocks.Head.Context.Delegates["tz1WnfXMPaNTBmH7DBPwqCWs9cPDJdkGBTZ8"].StakingBalance.GetAsync();

            Assert.NotNull(stakingBalance);
        }

        [Fact]
        public async Task TestContextNonces()
        {
            var head = await Rpc.Blocks.Head.Header.GetAsync();
            int level = head.level;

            //Add test for Rpc.Blocks.Head.Context.Nonces[level]
        }

        [Fact]
        public async Task TestContextSeed()
        {
            //Add test for Rpc.Blocks.Head.Context.Seed
        }
    }
}
