using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Netezos.Forging.Models;
using Netezos.Rpc;

namespace Netezos.Forging.Sandbox.Operations
{
    /// <summary>
    /// Create call to bake new block
    /// </summary>
    public class BakeBlockOperation : HeaderOperation
    {
        public FillOperation Fill(string blockId = "head") => new FillOperation(Rpc, Values, CallAsync, blockId, true);

        internal BakeBlockOperation(TezosRpc rpc, HeaderParameters headerParameters, string keyName, int minFee) : base(rpc, headerParameters)
        {
            headerParameters.Key = headerParameters.Keys.TryGetValue(keyName, out var key) 
                ? key
                : throw new KeyNotFoundException($"Parameter keyName {keyName} is not found");
            headerParameters.MinFee = minFee;
        }

        public override async Task<dynamic> CallAsync() => await CallAsync(Values);

        internal override async Task<ForwardingParameters> CallAsync(HeaderParameters parameters)
        {
            var pendingOperations = await Rpc.GetAsync<Dictionary<string, List<HeaderOperationContent>>>("/chains/main/mempool/pending_operations");

            pendingOperations.TryGetValue("applied", out var applied);

            var forwardingOperations = new List<List<HeaderOperationContent>>()
            {
                new List<HeaderOperationContent>(),
                new List<HeaderOperationContent>(),
                new List<HeaderOperationContent>(),
                new List<HeaderOperationContent>(),
            };

            applied?.Where(x =>
                {
                    if (x.Contents.FirstOrDefault()?.ValidationGroup != 3) return true;

                    var totalFee = x.Contents
                        .Select(y => y as ManagerOperationContent)
                        .Where(y => y != null)
                        .Sum(m => m.Fee);

                    return totalFee >= parameters.MinFee;
                })
                .ToList()
                .ForEach(x =>
                    forwardingOperations[(int) x.Contents.First().ValidationGroup].Add(
                        new HeaderOperationContent()
                        {
                            Branch = x.Branch,
                            Contents = x.Contents,
                            Hash = x.Hash,
                            Signature = x.Signature
                        })
                    );

            return new ForwardingParameters()
            {
                ShellHeader = new ShellHeaderContent(),
                BlockHeader = new BlockHeaderContent()
                {
                    ProtocolData = new ActivationProtocolDataContent()
                    {
                        //default
                        ProofOfWorkNonce = "0000000000000000"
                    }
                },
                Operations = forwardingOperations
            };
        }
    }
}