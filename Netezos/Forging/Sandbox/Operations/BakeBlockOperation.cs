using System;
using System.Threading.Tasks;
using Netezos.Forging.Models;
using Netezos.Keys;
using Netezos.Rpc;

namespace Netezos.Forging.Sandbox.Base
{
    /// <summary>
    /// Create call to bake new block
    /// </summary>
    public class BakeBlockOperation : HeaderOperation
    {
        public BakeBlockOperation(TezosRpc rpc, RequiredValues requiredValues, Func<RequiredValues, Task<(ShellHeaderContent, BlockHeaderContent, Signature)>> function) : base(rpc, requiredValues, function)
        {
        }

        public BakeBlockOperation(TezosRpc rpc, RequiredValues requiredValues) : base(rpc, requiredValues)
        {
        }

        public override Task<dynamic> ApplyAsync()
        {
            throw new NotImplementedException();
        }
        
        public async Task<(ShellHeaderContent, BlockHeaderContent, Signature)> Apply(RequiredValues data)
        {
            var pendingOperation = Rpc.GetAsync("/chains/main/mempool/pending_operations");
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