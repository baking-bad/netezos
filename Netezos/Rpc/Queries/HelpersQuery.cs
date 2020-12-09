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
        public BakingRightsQuery BakingRights => new BakingRightsQuery(this, "baking_rights/");

        /// <summary>
        /// Gets the query to the endorsing rights
        /// </summary>
        public EndorsingRightsQuery EndorsingRights => new EndorsingRightsQuery(this, "endorsing_rights/");
        
        /// <summary>
        /// Gets the query to the forging
        /// </summary>
        public ForgeQuery Forge => new ForgeQuery(this, "forge/");
        
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
        
        internal HelpersQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}
