using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class ProtocolParametersContent
    {
        [JsonPropertyName("bootstrap_accounts")]
        public List<List<string>> BootstrapAccounts { get; set; }
        [JsonPropertyName("bootstrap_contracts")]
        public List<object> BootstrapContracts { get; set; }
        [JsonPropertyName("commitments")]
        public List<List<string>> Commitments { get; set; }
        [JsonPropertyName("preserved_cycles")]
        public decimal PreservedCycles { get; set; }
        [JsonPropertyName("blocks_per_cycle")]
        public decimal BlocksPerCycle { get; set; }
        [JsonPropertyName("blocks_per_commitment")]
        public decimal BlocksPerCommitment { get; set; }
        [JsonPropertyName("blocks_per_roll_snapshot")]
        public decimal BlocksPerRollSnapshot { get; set; }
        [JsonPropertyName("blocks_per_voting_period")]
        public decimal BlocksPerVotingPeriod { get; set; }
        [JsonPropertyName("time_between_blocks")]
        public List<string> TimeBetweenBlocks { get; set; }
        [JsonPropertyName("endorsers_per_block")]
        public decimal EndorsersPerBlock { get; set; }
        [JsonPropertyName("hard_gas_limit_per_operation")]
        public string HardGasLimitPerOperation { get; set; }
        [JsonPropertyName("hard_gas_limit_per_block")]
        public string HardGasLimitPerBlock { get; set; }
        [JsonPropertyName("proof_of_work_threshold")]
        public string ProofOfWorkThreshold { get; set; }
        [JsonPropertyName("tokens_per_roll")]
        public string TokensPerRoll { get; set; }
        [JsonPropertyName("michelson_maximum_type_size")]
        public decimal MichelsonMaximumTypeSize { get; set; }
        [JsonPropertyName("seed_nonce_revelation_tip")]
        public string SeedNonceRevelationTip { get; set; }
        [JsonPropertyName("origination_size")]
        public decimal OriginationSize { get; set; }
        [JsonPropertyName("block_security_deposit")]
        public string BlockSecurityDeposit { get; set; }
        [JsonPropertyName("endorsement_security_deposit")]
        public string EndorsementSecurityDeposit { get; set; }
        [JsonPropertyName("baking_reward_per_endorsement")]
        public List<string> BakingRewardPerEndorsement { get; set; }
        [JsonPropertyName("endorsement_reward")]
        public List<string> EndorsementReward { get; set; }
        [JsonPropertyName("cost_per_byte")]
        public string CostPerByte { get; set; }
        [JsonPropertyName("hard_storage_limit_per_operation")]
        public string HardStorageLimitPerOperation { get; set; }
        [JsonPropertyName("test_chain_duration")]
        public string TestChainDuration { get; set; }
        [JsonPropertyName("quorum_min")]
        public decimal QuorumMin { get; set; }
        [JsonPropertyName("quorum_max")]
        public decimal QuorumMax { get; set; }
        [JsonPropertyName("min_proposal_quorum")]
        public decimal MinProposalQuorum { get; set; }
        [JsonPropertyName("initial_endorsers")]
        public decimal InitialEndorsers { get; set; }
        [JsonPropertyName("delay_per_missing_endorsement")]
        public string DelayPerMissingEndorsement { get; set; }
    }
}