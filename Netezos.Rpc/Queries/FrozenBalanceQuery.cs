namespace Netezos.Rpc.Queries
{
    /// <summary>
    /// Rpc query to get the frozen balances
    /// </summary>
    public class FrozenBalanceQuery : RpcObject
    {
        /// <summary>
        /// Gets the query to the frozen deposits
        /// </summary>
        public BlockQuery Deposits => new BlockQuery(this, "deposits/");

        /// <summary>
        /// Gets the query to the frozen fees
        /// </summary>
        public BlockQuery Fees => new BlockQuery(this, "fees/");

        /// <summary>
        /// Gets the query to the frozen rewards
        /// </summary>
        public BlockQuery Rewards => new BlockQuery(this, "rewards/");

        internal FrozenBalanceQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}
