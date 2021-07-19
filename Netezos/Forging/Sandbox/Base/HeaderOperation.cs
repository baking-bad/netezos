using System;
using System.Threading.Tasks;
using Netezos.Rpc;

namespace Netezos.Forging.Sandbox.Operations
{
    public abstract class HeaderOperation
    {
        internal readonly TezosRpc Rpc;

        /// <summary>
        /// Signature methods operation
        /// </summary>
        internal readonly Func<HeaderParameters, Task<ForwardingParameters>> Function;

        internal readonly HeaderParameters Values;

        internal HeaderOperation(
            TezosRpc rpc, 
            HeaderParameters headerParameters, 
            Func<HeaderParameters, Task<ForwardingParameters>> function = null)
        {
            Rpc = rpc;
            Values = headerParameters;
            Function = function;
        }

        public abstract Task<dynamic> CallAsync();

        internal abstract Task<ForwardingParameters> CallAsync(HeaderParameters values);
    }
}