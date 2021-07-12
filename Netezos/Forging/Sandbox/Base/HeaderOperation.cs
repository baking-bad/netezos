using System;
using System.Threading.Tasks;
using Netezos.Forging.Models;
using Netezos.Keys;
using Netezos.Rpc;

namespace Netezos.Forging.Sandbox.Base
{
    public abstract class HeaderOperation
    {
        internal readonly TezosRpc Rpc;

        /// <summary>
        /// Signature methods operation
        /// </summary>
        internal readonly Func<HeaderParameters, Task<(ShellHeaderContent, BlockHeaderContent, Signature)>> Function;

        internal readonly HeaderParameters Values;

        internal HeaderOperation(
            TezosRpc rpc, 
            HeaderParameters headerParameters, 
            Func<HeaderParameters, Task<(ShellHeaderContent, BlockHeaderContent, Signature)>> function = null)
        {
            Rpc = rpc;
            Values = headerParameters;
            Function = function;
        }

        public abstract Task<dynamic> CallAsync();

        protected abstract Task<(ShellHeaderContent, BlockHeaderContent, Signature)> CallAsync(HeaderParameters values);
    }

    /// <summary>
    /// Required values for operation blocks
    /// </summary>
    public class HeaderParameters
    {
        public string Key { get; set; }
        public string BlockId { get; set; } = "head";
        public string ProtocolHash { get; set; }
        public string ProtocolParameters { get; set; }
        public string Signature { get; set; }
        public int? MinFee { get; set; } = 0;
    }
}