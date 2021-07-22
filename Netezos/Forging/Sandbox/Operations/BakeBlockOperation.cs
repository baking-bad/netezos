using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Netezos.Forging.Models;
using Netezos.Rpc;

namespace Netezos.Forging.Sandbox.Operations
{
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

            var pendingOperations = await Rpc.Mempool.PendingOperations.GetAsync<Dictionary<string, List<HeaderOperationContent>>>();

            pendingOperations.TryGetValue("applied", out var applied);

            var forwardingOperations = new List<List<HeaderOperationContent>>()
            {
                new List<HeaderOperationContent>(),
                new List<HeaderOperationContent>(),
                new List<HeaderOperationContent>(),
                new List<HeaderOperationContent>(),
            };

            var operations = applied?.Where(x => 
                { 
                    if (x.Contents.FirstOrDefault()?.ValidationGroup != 3) return true;

                    var totalFee = x.Contents
                        .Select(y => y as ManagerOperationContent)
                        .Where(y => y != null)
                        .Sum(m => m.Fee);

                    return totalFee >= parameters.MinFee;
                });

            foreach (var operation in operations)
            {
                var index = (int) operation.Contents.First().ValidationGroup % forwardingOperations.Count;
                forwardingOperations[index].Add(operation);
            }
                
            return new ForwardingParameters()
            {
                BlockHeader = new BlockHeaderContent()
                {
                    ProtocolData = new ProtocolDataContent()
                    {
                        //default
                        ProofOfWorkNonce = "0000000000000000"
                    },
                    ShellHeader = new ShellHeaderContent()
                },
                Operations = forwardingOperations
            };
        }
    }
}