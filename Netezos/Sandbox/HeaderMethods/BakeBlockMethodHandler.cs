using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Netezos.Forging.Models;
using Netezos.Rpc;
using Netezos.Sandbox.Models;

namespace Netezos.Sandbox.HeaderMethods
{
    public class BakeBlockMethodHandler : HeaderMethodHandler
    {
        public FillMethodHandler Fill(string blockId = "head") => new FillMethodHandler(Rpc, Values, CallAsync, blockId, true);

        internal BakeBlockMethodHandler(TezosRpc rpc, HeaderParameters headerParameters, string keyName, int minFee) : base(rpc, headerParameters)
        {
            headerParameters.Key = headerParameters.Keys.TryGetValue(keyName, out var key) 
                ? key
                : throw new KeyNotFoundException($"Parameter keyName {keyName} is not found");
            headerParameters.MinFee = minFee;
        }

        public override async Task<dynamic> CallAsync() => await CallAsync(Values);

        internal override async Task<ForwardingParameters> CallAsync(HeaderParameters parameters)
        {

            
            var pendingOperations = await Rpc.Mempool.PendingOperations.GetAsync<MempoolOperations>();

            var forwardingOperations = new List<List<MempoolOperation>>()
            {
                new List<MempoolOperation>(),
                new List<MempoolOperation>(),
                new List<MempoolOperation>(),
                new List<MempoolOperation>()
            };

            var operations = pendingOperations.Applied?
                .Where(x => x != null)
                .Where(x => 
                {
                    if (x.Contents.FirstOrDefault()?.ValidationGroup != 3) return true;

                    var totalFee = x.Contents
                        .Select(y => y as ManagerOperationContent)
                        .Where(y => y != null)
                        .Sum(m => m.Fee);

                    return totalFee >= parameters.MinFee;
                }) ?? new List<Operation>();

            foreach (var operation in operations)
            {
                var index = (int) operation.Contents.First().ValidationGroup % forwardingOperations.Count;
                forwardingOperations[index].Add(new MempoolOperation()
                {
                    Protocol = parameters.ProtocolHash,
                    Branch = operation.Branch,
                    Contents = operation.Contents,
                    Signature = operation.Signature
                });
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