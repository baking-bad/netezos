namespace Netezos.Rpc.Queries
{
    /// <summary>
    /// Rpc query to access voting data
    /// </summary>
    public class VotesQuery : RpcQuery
    {
        /// <summary>
        /// Gets the query to the ballots casted so far during a voting period
        /// </summary>
        public RpcObject BallotList => new RpcObject(this, "ballot_list/");
        /// <summary>
        /// Gets the query to the sum of ballots casted so far during a voting period
        /// </summary>
        public RpcObject Ballots => new RpcObject(this, "ballots/");
        /// <summary>
        /// Gets the query to the current period kind
        /// </summary>
        public RpcObject CurrentPeriodKind => new RpcObject(this, "current_period_kind/");
        /// <summary>
        /// Gets the query to the current proposal under evaluation
        /// </summary>
        public RpcObject CurrentProposals => new RpcObject(this, "current_proposal/");
        /// <summary>
        /// Gets the query to the current expected quorum
        /// </summary>
        public RpcObject CurrentQuorum => new RpcObject(this, "current_quorum/");
        /// <summary>
        /// Gets the query to the list of delegates with their voting weight, in number of rolls
        /// </summary>
        public RpcObject Listings => new RpcObject(this, "listings/");
        /// <summary>
        /// Gets the query to the list of proposals with number of supporters
        /// </summary>
        public RpcObject Proposals => new RpcObject(this, "proposals/");

        internal VotesQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}
