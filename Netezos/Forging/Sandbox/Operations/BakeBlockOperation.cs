using System;
using System.Threading.Tasks;
using Dynamic.Json;
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
        internal BakeBlockOperation(TezosRpc rpc, RequiredValues requiredValues) : base(rpc, requiredValues) { }

        public override async Task<dynamic> ApplyAsync() => await Apply(Values);

        protected override async Task<(ShellHeaderContent, BlockHeaderContent, Signature)> Apply(RequiredValues data)
        {
            if (!data.MinFee.HasValue)
                throw new NullReferenceException($"MinFee parameter is required");

            var pendingOperations = await Rpc.GetAsync("/chains/main/mempool/pending_operations");

            var list = pendingOperations;
            
            
            return (null, null, null);
        }
    }
}