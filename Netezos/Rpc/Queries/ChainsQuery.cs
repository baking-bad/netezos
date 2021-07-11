using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Netezos.Rpc.Queries
{
    public class ChainsQuery : RpcObject
    {
        /// <summary>
        /// Forcefully set the bootstrapped flag of the node
        /// </summary>
        /// <param name="chainId"></param>
        /// <returns></returns>
        public ChainQuery this[string chainId] => new ChainQuery(this, $"{chainId}/");

        internal ChainsQuery(RpcClient client, string query) : base(client, query) { }
    }
}