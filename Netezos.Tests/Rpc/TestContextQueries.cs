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
        readonly string TestSmartRollup;
        readonly string KeyHash;
        readonly int BigMapId;

        public TestContextQueries(SettingsFixture settings)
        {
            Rpc = settings.Rpc;
            TestContract = settings.TestContract;
            TestEntrypoint = settings.TestEntrypoint;
            TestDelegate = settings.TestDelegate;
            TestSmartRollup = settings.TestSmartRollup;
            KeyHash = settings.KeyHash;
            BigMapId = settings.BigMapId;
        }

        [Fact]
        public async Task TestContextAdaptiveIssuanceLaunch()
        {
            var query = Rpc.Blocks.Head.Context.AdaptiveIssuanceLaunch;
            Assert.Equal($"chains/main/blocks/head/context/adaptive_issuance_launch_cycle/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
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
        public async Task TestContextAllTicketBalances()
        {
            var query = Rpc.Blocks.Head.Context.Contracts[TestContract].AllTicketBalances;
            Assert.Equal($"chains/main/blocks/head/context/contracts/{TestContract}/all_ticket_balances/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);
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
        public async Task TestContextContractBalanceAndFrozenBonds()
        {
            var query = Rpc.Blocks.Head.Context.Contracts[TestContract].BalanceAndFrozenBonds;
            Assert.Equal($"chains/main/blocks/head/context/contracts/{TestContract}/balance_and_frozen_bonds/", query.ToString());

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
            var query = Rpc.Blocks.Head.Context.Contracts[TestDelegate].Delegate;
            Assert.Equal($"chains/main/blocks/head/context/contracts/{TestDelegate}/delegate/", query.ToString());

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
        public async Task TestContextContractEstimatedOwnPendingSlashedAmount()
        {
            var query = Rpc.Blocks.Head.Context.Contracts[TestContract].EstimatedOwnPendingSlashedAmount;
            Assert.Equal($"chains/main/blocks/head/context/contracts/{TestContract}/estimated_own_pending_slashed_amount/", 
                query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
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
        public async Task TestContextContractFrozenBonds()
        {
            var query = Rpc.Blocks.Head.Context.Contracts[TestContract].FrozenBonds;
            Assert.Equal($"chains/main/blocks/head/context/contracts/{TestContract}/frozen_bonds/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestContextContractFullBalance()
        {
            var query = Rpc.Blocks.Head.Context.Contracts[TestContract].FullBalance;
            Assert.Equal($"chains/main/blocks/head/context/contracts/{TestContract}/full_balance/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
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
        public async Task TestContextContractStakedBalance()
        {
            var query = Rpc.Blocks.Head.Context.Contracts[TestDelegate].StakedBalance;
            Assert.Equal($"chains/main/blocks/head/context/contracts/{TestDelegate}/staked_balance/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestContextContractSpendable()
        {
            var query = Rpc.Blocks.Head.Context.Contracts[TestContract].Spendable;
            Assert.Equal($"chains/main/blocks/head/context/contracts/{TestContract}/spendable/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestContextContractSpendableAndFrozenBonds()
        {
            var query = Rpc.Blocks.Head.Context.Contracts[TestContract].SpendableAndFrozenBonds;
            Assert.Equal($"chains/main/blocks/head/context/contracts/{TestContract}/spendable_and_frozen_bonds/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
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
        public async Task TestContextContractPaidSpace()
        {
            var query = Rpc.Blocks.Head.Context.Contracts[TestContract].Storage.PaidSpace;
            Assert.Equal($"chains/main/blocks/head/context/contracts/{TestContract}/storage/paid_space/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestContextContractUsedSpace()
        {
            var query = Rpc.Blocks.Head.Context.Contracts[TestContract].Storage.UsedSpace;
            Assert.Equal($"chains/main/blocks/head/context/contracts/{TestContract}/storage/used_space/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestContextContractTicketBalance()
        {
            var query = Rpc.Blocks.Head.Context.Contracts[TestContract].TicketBalance;
            Assert.Equal($"chains/main/blocks/head/context/contracts/{TestContract}/ticket_balance/", query.ToString());

            var res = await query.PostAsync("KT1XtHJBXF3aAwggHrZh1fUbzaHc5MmB6QDs", 
                new
                {
                    prim = "int"
                },
                new
                {
                    @int = "1"
                });
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestContextContractUnstakeRequests()
        {
            var query = Rpc.Blocks.Head.Context.Contracts[TestDelegate].UnstakeRequests;
            Assert.Equal($"chains/main/blocks/head/context/contracts/{TestDelegate}/unstake_requests/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonObject || res == null);
        }

        [Fact]
        public async Task TestContextContractUnstakedFinalizableBalance()
        {
            var query = Rpc.Blocks.Head.Context.Contracts[TestDelegate].UnstakedFinalizableBalance;
            Assert.Equal($"chains/main/blocks/head/context/contracts/{TestDelegate}/unstaked_finalizable_balance/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestContextContractUnstakedFrozenBalance()
        {
            var query = Rpc.Blocks.Head.Context.Contracts[TestDelegate].UnstakedFrozenBalance;
            Assert.Equal($"chains/main/blocks/head/context/contracts/{TestDelegate}/unstaked_frozen_balance/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestContextDalCommitmentsHistory()
        {
            var query = Rpc.Blocks.Head.Context.Dal.CommitmentsHistory;
            Assert.Equal("chains/main/blocks/head/context/dal/commitments_history/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonObject);
        }

        [Fact]
        public async Task TestContextDalPublishedSlotHeaders()
        {
            var query = Rpc.Blocks.Head.Context.Dal.PublishedSlotHeaders;
            Assert.Equal("chains/main/blocks/head/context/dal/published_slot_headers/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);
        }

        [Fact]
        public async Task TestContextSkipListCellsOfLevel()
        {
            var query = Rpc.Blocks.Head.Context.Dal.SkipListCellsOfLevel;
            Assert.Equal("chains/main/blocks/head/context/dal/skip_list_cells_of_level/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);
        }

        [Fact]
        public async Task TestContextDalShards()
        {
            var query = Rpc.Blocks.Head.Context.Dal.Shards;
            Assert.Equal("chains/main/blocks/head/context/dal/shards/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);
        }

        [Fact]
        public async Task TestContextDelegates()
        {
            var query = Rpc.Blocks.Head.Context.Delegates;
            Assert.Equal($"chains/main/blocks/head/context/delegates/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);

            var delegateActiveRes = await query.GetAsync(DelegateStatus.Active);
            Assert.True(delegateActiveRes is DJsonArray);

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
        public async Task TestContextDelegateActiveStakingParameters()
        {
            var query = Rpc.Blocks.Head.Context.Delegates[TestDelegate].ActiveStakingParameters;
            Assert.Equal($"chains/main/blocks/head/context/delegates/{TestDelegate}/active_staking_parameters/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonObject);
        }

        [Fact]
        public async Task TestContextDelegateBakingPower()
        {
            var query = Rpc.Blocks.Head.Context.Delegates[TestDelegate].BakingPower;
            Assert.Equal($"chains/main/blocks/head/context/delegates/{TestDelegate}/baking_power/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestContextDelegateDelegators()
        {
            var query = Rpc.Blocks.Head.Context.Delegates[TestDelegate].Delegators;
            Assert.Equal($"chains/main/blocks/head/context/delegates/{TestDelegate}/delegators/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);
        }

        [Fact]
        public async Task TestContextDelegateExternalDelegated()
        {
            var query = Rpc.Blocks.Head.Context.Delegates[TestDelegate].ExternalDelegated;
            Assert.Equal($"chains/main/blocks/head/context/delegates/{TestDelegate}/external_delegated/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestContextDelegateExternalStaked()
        {
            var query = Rpc.Blocks.Head.Context.Delegates[TestDelegate].ExternalStaked;
            Assert.Equal($"chains/main/blocks/head/context/delegates/{TestDelegate}/external_staked/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestContextDelegateConsensusKey()
        {
            var query = Rpc.Blocks.Head.Context.Delegates[TestDelegate].ConsensusKey;
            Assert.Equal($"chains/main/blocks/head/context/delegates/{TestDelegate}/consensus_key/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonObject);
        }

        [Fact]
        public async Task TestContextDelegateCurrentVotingPower()
        {
            var query = Rpc.Blocks.Head.Context.Delegates[TestDelegate].CurrentVotingPower;
            Assert.Equal($"chains/main/blocks/head/context/delegates/{TestDelegate}/current_voting_power/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }
        
        [Fact]
        public async Task TestContextDalParticipation()
        {
            var query = Rpc.Blocks.Head.Context.Delegates[TestDelegate].DalParticipation;
            Assert.Equal($"chains/main/blocks/head/context/delegates/{TestDelegate}/dal_participation/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonObject);
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
        public async Task TestContextDelegateDenunciations()
        {
            var query = Rpc.Blocks.Head.Context.Delegates[TestDelegate].Denunciations;
            Assert.Equal($"chains/main/blocks/head/context/delegates/{TestDelegate}/denunciations/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);
        }

        [Fact]
        public async Task TestContextDelegateEstimatedSharedPendingSlashedAmount()
        {
            var query = Rpc.Blocks.Head.Context.Delegates[TestDelegate].EstimatedSharedPendingSlashedAmount;
            Assert.Equal($"chains/main/blocks/head/context/delegates/{TestDelegate}/estimated_shared_pending_slashed_amount/",
                query.ToString());

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
        public async Task TestContextDelegateIsForbidden()
        {
            var query = Rpc.Blocks.Head.Context.Delegates[TestDelegate].IsForbidden;
            Assert.Equal($"chains/main/blocks/head/context/delegates/{TestDelegate}/is_forbidden/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestContextDelegateMinDelegatedInCurrentCycle()
        {
            var query = Rpc.Blocks.Head.Context.Delegates[TestDelegate].MinDelegatedInCurrentCycle;
            Assert.Equal($"chains/main/blocks/head/context/delegates/{TestDelegate}/min_delegated_in_current_cycle/",
                query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonObject);
        }

        [Fact]
        public async Task TestContextDelegateOwnDelegated()
        {
            var query = Rpc.Blocks.Head.Context.Delegates[TestDelegate].OwnDelegated;
            Assert.Equal($"chains/main/blocks/head/context/delegates/{TestDelegate}/own_delegated/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestContextDelegateOwnFullBalance()
        {
            var query = Rpc.Blocks.Head.Context.Delegates[TestDelegate].OwnFullBalance;
            Assert.Equal($"chains/main/blocks/head/context/delegates/{TestDelegate}/own_full_balance/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestContextDelegateOwnStaked()
        {
            var query = Rpc.Blocks.Head.Context.Delegates[TestDelegate].OwnStaked;
            Assert.Equal($"chains/main/blocks/head/context/delegates/{TestDelegate}/own_staked/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestContextDelegateParticipation()
        {
            var query = Rpc.Blocks.Head.Context.Delegates[TestDelegate].Participation;
            Assert.Equal($"chains/main/blocks/head/context/delegates/{TestDelegate}/participation/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonObject);
        }

        [Fact]
        public async Task TestContextDelegatePendingStakingParameters()
        {
            var query = Rpc.Blocks.Head.Context.Delegates[TestDelegate].PendingStakingParameters;
            Assert.Equal($"chains/main/blocks/head/context/delegates/{TestDelegate}/pending_staking_parameters/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);
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
        public async Task TestContextDelegateTotalDelegated()
        {
            var query = Rpc.Blocks.Head.Context.Delegates[TestDelegate].TotalDelegated;
            Assert.Equal($"chains/main/blocks/head/context/delegates/{TestDelegate}/total_delegated/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestContextDelegateTotalStaked()
        {
            var query = Rpc.Blocks.Head.Context.Delegates[TestDelegate].TotalStaked;
            Assert.Equal($"chains/main/blocks/head/context/delegates/{TestDelegate}/total_staked/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestContextDelegateTotalUnstakedPerCycle()
        {
            var query = Rpc.Blocks.Head.Context.Delegates[TestDelegate].TotalUnstakedPerCycle;
            Assert.Equal($"chains/main/blocks/head/context/delegates/{TestDelegate}/total_unstaked_per_cycle/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);
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
        public async Task TestContextDelegateVotingInfo()
        {
            var query = Rpc.Blocks.Head.Context.Delegates[TestDelegate].VotingInfo;
            Assert.Equal($"chains/main/blocks/head/context/delegates/{TestDelegate}/voting_info/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonObject);
        }

        [Fact]
        public async Task TestContextIssuanceCurrentYearlyRate()
        {
            var query = Rpc.Blocks.Head.Context.Issuance.CurrentYearlyRate;
            Assert.Equal($"chains/main/blocks/head/context/issuance/current_yearly_rate/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestContextIssuanceCurrentYearlyRateDetails()
        {
            var query = Rpc.Blocks.Head.Context.Issuance.CurrentYearlyRateDetails;
            Assert.Equal($"chains/main/blocks/head/context/issuance/current_yearly_rate_details/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonObject);
        }

        [Fact]
        public async Task TestContextIssuanceCurrentYearlyRateExact()
        {
            var query = Rpc.Blocks.Head.Context.Issuance.CurrentYearlyRateExact;
            Assert.Equal($"chains/main/blocks/head/context/issuance/current_yearly_rate_exact/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonObject);
        }

        [Fact]
        public async Task TestContextIssuanceExpectedIssuance()
        {
            var query = Rpc.Blocks.Head.Context.Issuance.ExpectedIssuance;
            Assert.Equal($"chains/main/blocks/head/context/issuance/expected_issuance/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);
        }

        [Fact]
        public async Task TestContextIssuanceIssuancePerMinute()
        {
            var query = Rpc.Blocks.Head.Context.Issuance.IssuancePerMinute;
            Assert.Equal($"chains/main/blocks/head/context/issuance/issuance_per_minute/", query.ToString());

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

        [Fact]
        public async Task TestContextSmartRollupsAll()
        {
            var query = Rpc.Blocks.Head.Context.SmartRollups.All;
            Assert.Equal($"chains/main/blocks/head/context/smart_rollups/all/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);
        }

        [Fact]
        public async Task TestContextSmartRollupGenesisInfo()
        {
            var query = Rpc.Blocks.Head.Context.SmartRollups[TestSmartRollup].GenesisInfo;
            Assert.Equal($"chains/main/blocks/head/context/smart_rollups/smart_rollup/{TestSmartRollup}/genesis_info/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonObject);
        }

        [Fact]
        public async Task TestContextSmartRollupKind()
        {
            var query = Rpc.Blocks.Head.Context.SmartRollups[TestSmartRollup].Kind;
            Assert.Equal($"chains/main/blocks/head/context/smart_rollups/smart_rollup/{TestSmartRollup}/kind/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestContextSmartRollupLastCementedCommitmentHashWithLevel()
        {
            var query = Rpc.Blocks.Head.Context.SmartRollups[TestSmartRollup].LastCementedCommitmentHashWithLevel;
            Assert.Equal($"chains/main/blocks/head/context/smart_rollups/smart_rollup/{TestSmartRollup}/last_cemented_commitment_hash_with_level/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonObject);
        }

        [Fact]
        public async Task TestContextSmartRollupStakers()
        {
            var query = Rpc.Blocks.Head.Context.SmartRollups[TestSmartRollup].Stakers;
            Assert.Equal($"chains/main/blocks/head/context/smart_rollups/smart_rollup/{TestSmartRollup}/stakers/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonArray);
        }

        [Fact]
        public async Task TestContextTotalCurrentlyStaked()
        {
            var query = Rpc.Blocks.Head.Context.TotalCurrentlyStaked;
            Assert.Equal($"chains/main/blocks/head/context/total_currently_staked/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonObject);
        }

        [Fact]
        public async Task TestContextTotalFrozenStake()
        {
            var query = Rpc.Blocks.Head.Context.TotalFrozenStake;
            Assert.Equal($"chains/main/blocks/head/context/total_frozen_stake/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }

        [Fact]
        public async Task TestContextTotalSupply()
        {
            var query = Rpc.Blocks.Head.Context.TotalSupply;
            Assert.Equal($"chains/main/blocks/head/context/total_supply/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }
        
        [Fact]
        public async Task TestContextProtocolFirstLevel()
        {
            var query = Rpc.Blocks.Head.Context.Protocol.FirstLevel;
            Assert.Equal("chains/main/blocks/head/context/protocol/first_level/", query.ToString());

            var res = await query.GetAsync();
            Assert.True(res is DJsonValue);
        }
    }
}
