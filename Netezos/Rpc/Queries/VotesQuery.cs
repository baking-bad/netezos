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
        public RpcObject BallotList => new(this, "ballot_list");
        /// <summary>
        /// Gets the query to the sum of ballots casted so far during a voting period
        /// </summary>
        public RpcObject Ballots => new(this, "ballots");
        /// <summary>
        /// Gets the query to the current period kind
        /// </summary>
        [Obsolete("This RPC query was removed. Use it on early protocols only.")]
        public RpcObject CurrentPeriodKind => new(this, "current_period_kind");
        /// <summary>
        /// Gets the query to the current proposal under evaluation
        /// </summary>
        public RpcObject CurrentProposals => new(this, "current_proposal");
        /// <summary>
        /// Gets the query to the current expected quorum
        /// </summary>
        public RpcObject CurrentQuorum => new(this, "current_quorum");
        /// <summary>
        /// Gets the query to the list of delegates with their voting weight, in number of rolls
        /// </summary>
        public RpcObject Listings => new(this, "listings");
        /// <summary>
        /// Gets the query to the list of proposals with number of supporters
        /// </summary>
        public RpcObject Proposals => new(this, "proposals");

        internal VotesQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}
