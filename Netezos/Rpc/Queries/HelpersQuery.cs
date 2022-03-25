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
        public BakingRightsQuery BakingRights => new BakingRightsQuery(this, "baking_rights");
        
        /// <summary>
        /// Returns the level of the interrogated block.
        /// </summary>
        public RpcObject CurrentLevel => new RpcObject(this, "current_level");

        /// <summary>
        /// Gets the query to the endorsing rights
        /// </summary>
        public EndorsingRightsQuery EndorsingRights => new EndorsingRightsQuery(this, "endorsing_rights");
        
        /// <summary>
        /// Gets the query to the forging
        /// </summary>
        public ForgeQuery Forge => new ForgeQuery(this, "forge/");
        
        /// <summary>
        /// Levels of a cycle
        /// </summary>
        public RpcObject LevelsInCurrentCycle => new RpcObject(this, "levels_in_current_cycle");
        
        /// <summary>
        /// Gets the query to the parsing
        /// </summary>
        public ParseQuery Parse => new ParseQuery(this, "parse/");

        /// <summary>
        /// Gets the query to the preapplying
        /// </summary>
        public PreapplyQuery Preapply => new PreapplyQuery(this, "preapply/");  
        
        /// <summary>
        /// Gets the query to scripts
        /// </summary>
        public ScriptsQuery Scripts => new ScriptsQuery(this, "scripts/");
        
        /// <summary>
        /// Gets the query to validators
        /// </summary>
        public ValidatorsQuery Validators => new ValidatorsQuery(this, "validators");
        
        internal HelpersQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}
