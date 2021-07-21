using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Netezos.Encoding;
using Netezos.Forging.Models;
using Netezos.Keys;
using Netezos.Rpc;

namespace Netezos.Forging.Sandbox.Operations
{
    /// <summary>
    /// Inject the signed block header
    /// </summary>
    public class InjectOperation : HeaderOperation
    {
        internal InjectOperation(
            TezosRpc rpc,
            HeaderParameters headerParameters,
            Func<HeaderParameters, Task<ForwardingParameters>> action) 
            : base(rpc, headerParameters, action) { }

        /// <summary>
        /// Returned hash block
        /// </summary>
        /// <returns></returns>
        public override async Task<dynamic> CallAsync()
        {
            var parameters = await Function(Values);

            var data = Hex.Convert(LocalForge.ForgeBinaryPayload(
                parameters.BlockHeader.ShellHeader,
                parameters.BlockHeader.ProtocolData,
                parameters.Signature)
            );

            var hash = await Rpc.Inject.Block.PostAsync<string>(data, 
                parameters.Operations?.Select(x => x.Select(y => (object)y)) 
                        ?? new List<List<object>>(), 
                force:true, 
                async:false
            );

            return hash;
        }

        internal override Task<ForwardingParameters> CallAsync(HeaderParameters values)
        {
            throw new NotImplementedException("Inject operation not have next operation");
        }
    }
}