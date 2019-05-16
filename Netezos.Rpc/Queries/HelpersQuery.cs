namespace Netezos.Rpc.Queries
{
    /// <summary>
    /// Rpc query to get helpers assotiated with a block
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

        internal HelpersQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}
