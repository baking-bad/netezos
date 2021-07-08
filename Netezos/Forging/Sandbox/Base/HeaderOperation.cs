﻿using System;
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
        /// 
        /// </summary>
        internal readonly Func<RequiredValues, Task<(ShellHeaderContent, BlockHeaderContent, Signature)>> Function;

        internal readonly RequiredValues Values;

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

        protected abstract Task<(ShellHeaderContent, BlockHeaderContent, Signature)> Apply(RequiredValues values);
    }

    public class RequiredValues
    {
        public string Key { get; set; }
        public string BlockId { get; set; } = "head";
        public string ProtocolHash { get; set; }
        public string Signature { get; set; }
    }
}