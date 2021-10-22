using System;
using System.Linq;
using System.Threading.Tasks;
using Netezos.Encoding;
using Netezos.Forging;
using Netezos.Keys;
using Netezos.Rpc;
using Netezos.Sandbox.Base;
using Netezos.Sandbox.Models;

namespace Netezos.Sandbox.BlockMethods
{
    public class SignMethodHandler : BlockMethodHandler
    {
        internal SignMethodHandler(
            TezosRpc rpc, 
            BlockParameters headerParameters, 
            Func<BlockParameters, Task<ForwardingParameters>> function = null) 
            : base(rpc, headerParameters, function)
        { }

        public InjectMethodHandler Inject => new InjectMethodHandler(Rpc, Values, CallAsync);

        public override async Task<dynamic> CallAsync() => await CallAsync(Values);

        internal override async Task<ForwardingParameters> CallAsync(BlockParameters values)
        {
            var parameters = await Function(values);
            var validationPass = values.Operations[0].ValidationGroup;
            if (values.Operations.Any(x => x.ValidationGroup != validationPass))
                throw new Exception("Mixed validation passes");

            byte[] watermark;
            if (validationPass == 0)
            {
                if (string.IsNullOrEmpty(values.ChainId))
                    throw new NullReferenceException("Chain ID is undefined, run .fill first'");
                watermark = new byte[] {2}.Concat(Base58.Parse(values.ChainId, 3));
            }
            else
            {
                watermark = new byte[] {3};
            }

            // var bytes = await new LocalForge().ForgeOperationGroupAsync(values.Branch, values.Operations);
            var forgedData = await Rpc
                .Blocks[values.Branch]
                .Helpers
                .Forge
                .Operations
                .PostAsync<string>(values.Branch, values.Operations.Select(x => (object)x).ToList());
            parameters.Signature = values.Key.SignOperation(Hex.Parse(forgedData), true);
            values.Signature = values.Key.SignOperation(Hex.Parse(forgedData), true);
            return parameters;
        }
    }
}