using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Netezos.Encoding;
using Netezos.Forging;
using Netezos.Rpc;
using Netezos.Sandbox.Models;

namespace Netezos.Sandbox.HeaderMethods
{
    /// <summary>
    /// Inject the signed block header
    /// </summary>
    public class InjectMethodHandler : HeaderMethodHandler
    {
        internal InjectMethodHandler(
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