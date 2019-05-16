﻿namespace Netezos.Rpc.Queries
{
    /// <summary>
    /// Rpc query to access contract data
    /// </summary>
    public class ContractQuery : RpcObject
    {
        /// <summary>
        /// Gets the query to the balance of a contract
        /// </summary>
        public RpcObject Balance => new RpcObject(this, "balance/");

        /// <summary>
        /// Gets the query to the value associated with a key in the big map storage of the contract
        /// </summary>
        public RpcObject BigMap => new RpcObject(this, "big_map_get/");

        /// <summary>
        /// Gets the query to the counter of a contract, if any
        /// </summary>
        public RpcObject Counter => new RpcObject(this, "counter/");

        /// <summary>
        /// Gets the query to the flag, indicating if the contract delegate can be changed
        /// </summary>
        public RpcObject Delegatable => new RpcObject(this, "delegatable/");

        /// <summary>
        /// Gets the query to the delegate of a contract, if any
        /// </summary>
        public RpcObject Delegate => new RpcObject(this, "delegate/");

        /// <summary>
        /// Gets the query to the manager of a contract
        /// </summary>
        public RpcObject Manager => new RpcObject(this, "manager/");

        /// <summary>
        /// Gets the query to the manager of a contract and his key
        /// </summary>
        public RpcObject ManagerKey => new RpcObject(this, "manager_key/");

        /// <summary>
        /// Gets the query to the code and data of the contract
        /// </summary>
        public RpcObject Script => new RpcObject(this, "script/");

        /// <summary>
        /// Gets the query to the flag, indicating if the contract tokens can be spent by the manager
        /// </summary>
        public RpcObject Spendable => new RpcObject(this, "spendable/");

        /// <summary>
        /// Gets the query to the data of the contract's storage
        /// </summary>
        public RpcObject Storage => new RpcObject(this, "storage/");

        internal ContractQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}
