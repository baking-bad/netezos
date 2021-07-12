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
            HeaderParameters headerParameters, 
            Func<HeaderParameters, Task<(ShellHeaderContent, BlockHeaderContent, Signature)>> function) 
            : base(rpc, headerParameters, function) { }

        public override Task<dynamic> CallAsync()
        {
            throw new NotImplementedException();
        }

        protected override Task<(ShellHeaderContent, BlockHeaderContent, Signature)> CallAsync(HeaderParameters values)
        {
            throw new NotImplementedException();
        }
    }
}