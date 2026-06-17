namespace Netezos.Rpc.Queries
{
    /// <summary>
    /// Rpc query to access sTEZ data
    /// </summary>
    public class StezQuery : RpcQuery
    {
        /// <summary>
        /// List of delegates registered with sTEZ together with their fee and capacity.
        /// </summary>
        public RpcObject Bakers => new(this, "bakers/");

        /// <summary>
        /// Returns the sTEZ contract hash.
        /// </summary>
        public RpcObject ContractHash => new(this, "contract_hash/");

        /// <summary>
        /// Returns the exchange rate between sTEZ token and tez calculated as the ratio of total_amount_of_tez to total_supply.
        /// Returns {1, 1} if total_supply is 0.
        /// </summary>
        public RpcObject ExchangeRate => new(this, "exchange_rate/");

        /// <summary>
        /// List of delegates with their staking power from sTEZ for the current cycle.
        /// </summary>
        public RpcObject StakingPower => new(this, "staking_power/");

        /// <summary>
        /// Returns the total amount of tez in the sTEZ staking ledger (in mutez).
        /// </summary>
        public RpcObject TotalAmountOfTez => new(this, "total_amount_of_tez/");

        /// <summary>
        /// Returns the total supply of sTEZ tokens.
        /// </summary>
        public RpcObject TotalSupply => new(this, "total_supply/");

        internal StezQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}