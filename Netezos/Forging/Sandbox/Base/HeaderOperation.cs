using System;
using System.Threading.Tasks;
using Netezos.Forging.Models;
using Netezos.Keys;
using Netezos.Rpc;

namespace Netezos.Forging.Sandbox.Base
{
    public abstract class HeaderOperation
    {
        protected readonly TezosRpc Rpc;
        /// <summary>
        /// 
        /// </summary>
        protected readonly Func<RequiredValues, Task<(ShellHeaderContent, BlockHeaderContent, Signature)>> Function;

        protected readonly RequiredValues Values;

        internal HeaderOperation(TezosRpc rpc, RequiredValues requiredValues, Func<RequiredValues, Task<(ShellHeaderContent, BlockHeaderContent, Signature)>> function)
        {
            Rpc = rpc;
            Values = requiredValues;
            Function = function;
        }

        internal HeaderOperation(TezosRpc rpc, RequiredValues requiredValues)
        {
            Rpc = rpc;
            Values = requiredValues;
            Function = null;
        }

        public abstract Task<dynamic> ApplyAsync();
    }

    public class RequiredValues
    {
        public string Key { get; set; }
        public string BlockId { get; set; } = "head";
        public string ProtocolHash { get; set; }
        public string Signature { get; set; }
    }
}