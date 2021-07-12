using System;
using System.Threading.Tasks;
using Netezos.Forging.Models;
using Netezos.Keys;
using Netezos.Rpc;

namespace Netezos.Forging.Sandbox.Base
{
    public class WorkOperation : HeaderOperation
    {
        /// <summary>
        /// Perform calculations to find proof-of-work nonce
        /// </summary>
        public WorkOperation(
            TezosRpc rpc, 
            RequiredValues requiredValues, 
            Func<RequiredValues, Task<(ShellHeaderContent, BlockHeaderContent, Signature)>> function) 
            : base(rpc, requiredValues, function) { }

        public override Task<dynamic> ApplyAsync()
        {
            throw new NotImplementedException();
        }

        protected override Task<(ShellHeaderContent, BlockHeaderContent, Signature)> Apply(RequiredValues values)
        {
            throw new NotImplementedException();
        }
    }
}