using System;
using System.Threading.Tasks;
using Netezos.Rpc;
using Netezos.Sandbox.Base;
using Netezos.Sandbox.Models;

namespace Netezos.Sandbox
{
    public abstract class HeaderMethodHandler : MethodHandler<HeaderParameters, ForwardingParameters>
    {
        internal HeaderMethodHandler(
            TezosRpc rpc, 
            HeaderParameters headerParameters, 
            Func<HeaderParameters, Task<ForwardingParameters>> function = null) 
            : base(rpc, headerParameters, function) 
        { }
    }
}