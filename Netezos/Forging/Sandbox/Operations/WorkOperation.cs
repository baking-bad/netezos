using System;
using System.Threading.Tasks;
using Netezos.Forging.Models;
using Netezos.Keys;
using Netezos.Rpc;

namespace Netezos.Forging.Sandbox.Base
{
    public class WorkOperation : HeaderOperation
    {
        public WorkOperation(TezosRpc rpc, RequiredValues requiredValues, Func<RequiredValues, Task<(ShellHeaderContent, BlockHeaderContent, Signature)>> function) : base(rpc, requiredValues, function)
        {
        }

        public WorkOperation(TezosRpc rpc, RequiredValues requiredValues) : base(rpc, requiredValues)
        {
        }

        /// <summary>
        /// Perform calculations to find proof-of-work nonce
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override Task<dynamic> ApplyAsync()
        {
            throw new NotImplementedException();
        }
    }
}