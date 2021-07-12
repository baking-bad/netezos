using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Netezos.Encoding;
using Netezos.Forging.Models;
using Netezos.Keys;
using Netezos.Rpc;

namespace Netezos.Forging.Sandbox.Base
{
    /// <summary>
    /// Inject the signed block header
    /// </summary>
    public class InjectOperation : HeaderOperation
    {
        internal InjectOperation(
            TezosRpc rpc,
            HeaderParameters headerParameters,
            Func<HeaderParameters, Task<(ShellHeaderContent, BlockHeaderContent, Signature)>> action) 
            : base(rpc, headerParameters, action) { }

        /// <summary>
        /// Returned hash block
        /// </summary>
        /// <returns></returns>
        public override async Task<dynamic> CallAsync()
        {
            var (shell, blockHeader, signature) = await Function.Invoke(Values);

            var data = Hex.Convert(LocalForge.ForgeBinaryPayload(shell, blockHeader.ProtocolData, signature));
            var hash = await Rpc.Inject.Block.PostAsync<string>(data, 
                blockHeader
                    .Operations?
                    .Select(x => x?
                        .Contents?  
                        .Select(y => (object)y)
                        .ToList())
                    .ToList()
                ?? new List<List<object>>(), 
                force:true, 
                async:false
            );

            return hash;
        }

        protected override Task<(ShellHeaderContent, BlockHeaderContent, Signature)> CallAsync(HeaderParameters values)
        {
            throw new NotImplementedException("Inject operation not have next operation");
        }
    }
}