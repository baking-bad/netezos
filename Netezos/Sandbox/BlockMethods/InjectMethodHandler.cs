using System;
using System.Linq;
using System.Threading.Tasks;
using Netezos.Encoding;
using Netezos.Forging.Models;
using Netezos.Rpc;
using Netezos.Sandbox.Base;
using Netezos.Sandbox.Models;

namespace Netezos.Sandbox.BlockMethods
{
    public class InjectMethodHandler : BlockMethodHandler
    {
        internal InjectMethodHandler(TezosRpc rpc, 
            BlockParameters headerParameters, 
            Func<BlockParameters, Task<ForwardingParameters>> function = null) 
            : base(rpc, headerParameters, function)
        {
        }

        public override async Task<dynamic> CallAsync()
        {
            var parameters = await Function(Values);
            var binaryPayload = await BinaryPayload();
            return await Rpc.Inject.Operation.PostAsync<dynamic>(binaryPayload, async:false);
        }

        private async Task<byte[]> BinaryPayload()
        {
            var forgedData = await Rpc
                .Blocks[Values.Branch]
                .Helpers
                .Forge
                .Operations
                .PostAsync<string>(Values.Branch, Values.Operations.Select(x => (object)x).ToList());

            if (Values.Operations[0].Kind == "endorsement_with_slot")
                return Hex.Parse(forgedData).Concat(new byte[64]);

            if (Values.Signature == null)
                throw new NullReferenceException("Not Signed");

            return Hex.Parse(forgedData).Concat(Base58.Parse(Values.Signature, 5));
        }

        internal override Task<ForwardingParameters> CallAsync(BlockParameters values)
        {
            throw new NotImplementedException("Inject operation not have next operation");
        }

    }
}