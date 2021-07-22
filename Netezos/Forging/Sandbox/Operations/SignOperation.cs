using System;
using System.Threading.Tasks;
using Netezos.Encoding;
using Netezos.Forging.Models;
using Netezos.Keys;
using Netezos.Rpc;

namespace Netezos.Forging.Sandbox.Operations
{
    /// <summary>
    /// Sign the block header with the specified key
    /// </summary>
    public class SignOperation : HeaderOperation
    {
        internal SignOperation(
            TezosRpc rpc,
            HeaderParameters headerParameters,
            Func<HeaderParameters, Task<ForwardingParameters>> function) 
            : base(rpc, headerParameters, function) { }
        
        public InjectOperation InjectBlock => new InjectOperation(Rpc, Values, CallAsync);

        public override async Task<dynamic> CallAsync() => await CallAsync(Values);

        internal override async Task<ForwardingParameters> CallAsync(HeaderParameters data)
        {
            var parameters = await Function(data);

            var chainId = await Rpc.ChainId.GetAsync<string>();

            var watermark = new byte[] {1}.Concat(Base58.Parse(chainId, 3));
            parameters.Signature  = Key.FromBase58(data.Key).Sign(
                LocalForge.Concat(
                    watermark, 
                    LocalForge.ForgeHeaderValues(parameters.BlockHeader.ShellHeader, parameters.BlockHeader.ProtocolData)
                    )
                );

            return parameters;

        }
        
    }
}