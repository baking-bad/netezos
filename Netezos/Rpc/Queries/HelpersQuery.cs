namespace Netezos.Rpc.Queries
{
    /// <summary>
    /// RPC query to get helpers associated with a block
    /// </summary>
    public class HelpersQuery : RpcQuery
    {
        /// <summary>
        /// Gets the query to the baking rights
        /// </summary>
        public BakingRightsQuery BakingRights => new(this, "baking_rights");
        
        /// <summary>
        /// Returns the level of the interrogated block.
        /// </summary>
        public RpcObject CurrentLevel => new(this, "current_level");

        /// <summary>
        /// Gets the query to the attestation rights
        /// </summary>
        public AttestationRightsQuery AttestationRights => new(this, "attestation_rights");

        /// <summary>
        /// Gets the query to the forging
        /// </summary>
        public ForgeQuery Forge => new(this, "forge/");
        
        /// <summary>
        /// Levels of a cycle
        /// </summary>
        public RpcObject LevelsInCurrentCycle => new(this, "levels_in_current_cycle");
        
        /// <summary>
        /// Gets the query to the parsing
        /// </summary>
        public ParseQuery Parse => new(this, "parse/");

        /// <summary>
        /// Gets the query to the pre-applying
        /// </summary>
        public PreapplyQuery Preapply => new(this, "preapply/");  
        
        /// <summary>
        /// Gets the query to scripts
        /// </summary>
        public ScriptsQuery Scripts => new(this, "scripts/");
        
        /// <summary>
        /// Gets the query to validators
        /// </summary>
        public ValidatorsQuery Validators => new(this, "validators");

        /// <summary>
        /// Returns the number of blocks consecutively baked at round zero.
        /// </summary>
        public RpcObject ConsecutiveRoundZero => new(this, "consecutive_round_zero");

        /// <summary>
        /// Returns the total baking power for the current cycle.
        /// </summary>
        public RpcObject TotalBakingPower => new(this, "total_baking_power");

        internal HelpersQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}
