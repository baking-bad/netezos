using System;
using System.Threading.Tasks;
using Netezos.Rpc;

namespace Netezos.Sandbox.Base
{
    public abstract class MethodHandler<T, D>
    {
        internal readonly TezosRpc Rpc;

        /// <summary>
        /// Signature methods operation
        /// </summary>
        internal readonly Func<T, Task<D>> Function;

        internal readonly T Values;

        internal MethodHandler(
            TezosRpc rpc, 
            T parameters, 
            Func<T, Task<D>> function = null)
        {
            Rpc = rpc;
            Values = parameters;
            Function = function;
        }

        public abstract Task<dynamic> CallAsync();

        internal abstract Task<D> CallAsync(T values);
    }
}