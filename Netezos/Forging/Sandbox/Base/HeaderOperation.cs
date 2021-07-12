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
        internal readonly Func<RequiredValues, Task<(ShellHeaderContent, BlockHeaderContent, Signature)>> Function;

        internal readonly RequiredValues Values;

        internal HeaderOperation(TezosRpc rpc, RequiredValues requiredValues, Func<RequiredValues, Task<(ShellHeaderContent, BlockHeaderContent, Signature)>> function = null)
        {
            Rpc = rpc;
            Values = requiredValues;
            Function = function;
        }

        public abstract Task<dynamic> ApplyAsync();

        protected abstract Task<(ShellHeaderContent, BlockHeaderContent, Signature)> Apply(RequiredValues values);
    }

    /// <summary>
    /// Required values for operation blocks
    /// </summary>
    public class RequiredValues
    {
        public string Key { get; set; }
        public string BlockId { get; set; } = "head";
        public string ProtocolHash { get; set; }
        public string ProtocolParameters { get; set; }
        public string Signature { get; set; }
        public int? MinFee { get; set; } = 0;

        internal byte[] Forge()
        {
            return null;
        }
    }
}