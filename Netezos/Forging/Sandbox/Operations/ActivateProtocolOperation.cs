using System;
using System.Threading.Tasks;
using Netezos.Forging.Models;
using Netezos.Keys;
using Netezos.Rpc;

namespace Netezos.Forging.Sandbox.Base
{
    public class ActivateProtocolOperation : HeaderOperation
    {
        public FillOperation Fill => new FillOperation(Rpc, Values, Apply);
        
        public ActivateProtocolOperation(
            TezosRpc rpc, 
            RequiredValues requiredValues) : base(rpc, requiredValues)
        {
        }
        
        public override async Task<dynamic> ApplyAsync() => await Apply(Values);

        protected override async Task<(ShellHeaderContent, BlockHeaderContent, Signature)> Apply(RequiredValues data)
        {
            var header = await Rpc.Blocks.Head.Header.Shell.GetAsync<ShellHeaderContent>();
            var fitness = header.Fitness.BumpFitness();
            var blockHeader = new BlockHeaderContent()
            {
                ProtocolData = new ActivationProtocolDataContent()
                {
                    Content = new ActivationCommandContent()
                    {
                        Hash = data.ProtocolHash,
                        Fitness = fitness,
                        ProtocolParameters = data.ProtocolHash
                    }
                }
            };
            return (null, blockHeader, null);
        }
    }
}