using System;
using System.Threading.Tasks;
using Netezos.Rpc;
using Netezos.Sandbox.Base;
using Netezos.Sandbox.Models;

namespace Netezos.Sandbox
{
    public abstract class BlockMethodHandler : MethodHandler<BlockParameters, ForwardingParameters>
    {
        internal BlockMethodHandler(TezosRpc rpc, 
            BlockParameters parameters, 
            Func<BlockParameters, Task<ForwardingParameters>> function = null) 
            : base(rpc, parameters, function)
        {
        }
    }
}