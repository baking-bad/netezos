namespace Netezos.Rpc.Queries
{
    /// <summary>
    /// Rpc query to access all existing contracts
    /// </summary>
    public class ContractsQuery : RpcObject
    {
        /// <summary>
        /// Gets the query to the complete status of a contract by address
        /// </summary>
        /// <param name="address">Address of the contract</param>
        /// <returns></returns>
        public ContractQuery this[string address] => new ContractQuery(this, $"{address}/");

        internal ContractsQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}
