using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class ConstantsContent
    {
        [JsonPropertyName("proof_of_work_nonce_size")]
        public int ProofOfWorkNonceSize { get; set; }

        [JsonPropertyName("nonce_length")]
        public int NonceLength { get; set; }

        [JsonPropertyName("max_anon_ops_per_block")]
        public int MaxAnonOpsPerBlock { get; set; }

        [JsonPropertyName("max_operation_data_length")]
        public int MaxOperationDataLength { get; set; }

        [JsonPropertyName("max_proposals_per_delegate")]
        public int MaxProposalsPerDelegate { get; set; }

        [JsonPropertyName("preserved_cycles")]
        public int PreservedCycles { get; set; }

        [JsonPropertyName("blocks_per_cycle")]
        public int BlocksPerCycle { get; set; }

        [JsonPropertyName("blocks_per_commitment")]
        public int BlocksPerCommitment { get; set; }

        [JsonPropertyName("blocks_per_roll_snapshot")]
        public int BlocksPerRollSnapshot { get; set; }

        [JsonPropertyName("blocks_per_voting_period")]
        public int BlocksPerVotingPeriod { get; set; }

        [JsonPropertyName("time_between_blocks")]
        public List<string> TimeBetweenBlocks { get; set; }

        [JsonPropertyName("endorsers_per_block")]
        public int EndorsersPerBlock { get; set; }

        [JsonPropertyName("hard_gas_limit_per_operation")]
        public int HardGasLimitPerOperation { get; set; }

        [JsonPropertyName("hard_gas_limit_per_block")]
        public string HardGasLimitPerBlock { get; set; }

        [JsonPropertyName("proof_of_work_threshold")]
        public string ProofOfWorkThreshold { get; set; }

        [JsonPropertyName("tokens_per_roll")]
        public string TokensPerRoll { get; set; }

        [JsonPropertyName("michelson_maximum_type_size")]
        public int MichelsonMaximumTypeSize { get; set; }

        [JsonPropertyName("seed_nonce_revelation_tip")]
        public string SeedNonceRevelationTip { get; set; }

        [JsonPropertyName("origination_size")]
        public int OriginationSize { get; set; }

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
        public int HardStorageLimitPerOperation { get; set; }

        [JsonPropertyName("test_chain_duration")]
        public string TestChainDuration { get; set; }

        [JsonPropertyName("quorum_min")]
        public int QuorumMin { get; set; }

        [JsonPropertyName("quorum_max")]
        public int QuorumMax { get; set; }

        [JsonPropertyName("min_proposal_quorum")]
        public int MinProposalQuorum { get; set; }

        [JsonPropertyName("initial_endorsers")]
        public int InitialEndorsers { get; set; }

        [JsonPropertyName("delay_per_missing_endorsement")]
        public string DelayPerMissingEndorsement { get; set; }
    }
}