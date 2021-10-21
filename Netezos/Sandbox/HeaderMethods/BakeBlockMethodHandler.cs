using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Netezos.Forging.Models;
using Netezos.Keys;
using Netezos.Rpc;
using Netezos.Sandbox.Base;
using Netezos.Sandbox.Models;

namespace Netezos.Sandbox.HeaderMethods
{
    public class BakeBlockMethodHandler : HeaderMethodHandler, IBakeBlockHandler
    {
        /// <summary>
        /// Filling missing fields essential for preapply 
        /// </summary>
        /// <param name="blockId">"head" or "genesis"</param>
        /// <returns>Header method handler</returns>
        public FillMethodHandler Fill(string blockId = "head") => new FillMethodHandler(Rpc, Values, CallAsync, blockId, true);

        internal BakeBlockMethodHandler(TezosRpc rpc, HeaderParameters parameters) : base(rpc, parameters)
        {
        }


        public override async Task<dynamic> CallAsync() => await CallAsync(Values);

        internal override async Task<ForwardingParameters> CallAsync(HeaderParameters parameters)
        {

            var pendingOperations = await Rpc.Mempool.PendingOperations.GetAsync<MempoolOperations>();

            var forwardingOperations = new List<MempoolOperation>[4]
                .Select(x => new List<MempoolOperation>())
                .ToList();

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