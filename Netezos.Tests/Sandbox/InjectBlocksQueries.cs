using System.Threading.Tasks;
using Dynamic.Json;
using Netezos.Forging.Models;
using Netezos.Rpc;
using Netezos.Sandbox;
using Netezos.Tests.Rpc;
using Xunit;

namespace Netezos.Tests.Sandbox
{
    public class InjectBlocksQueries
    {
        readonly TezosRpc Rpc;
        readonly BlockOperationsClient OperationClient;
        readonly BlockHeaderClient HeaderClient;
        
        public InjectBlocksQueries(SettingsFixture settings)
        {
            OperationClient = settings.BlockOperationsClient;
            HeaderClient = settings.BlockHeaderClient;
            Rpc = settings.Rpc;
        }

        [Fact]
        public async Task TestActivateProtocol()
        {
            OperationClient.Clear();
            OperationClient.Add(new ActivationContent());
            var result = await OperationClient.Fill.Sign.Inject.CallAsync();
            var hash = await HeaderClient.BakeBlock("bootstrap1").Fill().Work.Sign.InjectBlock.CallAsync();

            var balance = await Rpc.Blocks.Head.Context.Contracts["tz1W86h1XuWy6awbNUTRUgs6nk8q5vqXQwgk"].Balance.GetAsync<string>();
            Assert.Equal("100500000000", balance);
        }

        [Fact]
        public async Task RevealPublicKeyHashSendTez()
        {
            OperationClient.Clear();
            OperationClient.Add(new RevealContent());
            OperationClient.Add(new TransactionContent() {Destination = "edsk3gUfUPyBSfrS9CCgmCiQsTCHGkviBDusMxDJstFtojtc1zcpsh", Amount = 23});
            var result = await OperationClient.Fill.Sign.Inject.CallAsync();
            var hash = await HeaderClient.BakeBlock("bootstrap1").Fill().Work.Sign.InjectBlock.CallAsync();

            var balance = await Rpc.Blocks.Head.Context.Contracts["edsk3gUfUPyBSfrS9CCgmCiQsTCHGkviBDusMxDJstFtojtc1zcpsh"].Balance.GetAsync<string>();
            Assert.Equal("4000000000023", balance);
        }

        [Fact]
        public async Task SendMultipleTransactions()
        {
            OperationClient.Clear();
            OperationClient.Add(new TransactionContent() {Destination = "edsk4ArLQgBTLWG5FJmnGnT689VKoqhXwmDPBuGx3z4cvwU9MmrPZZ", Amount = 23});
            OperationClient.Add(new TransactionContent() {Destination = "edsk4ArLQgBTLWG5FJmnGnT689VKoqhXwmDPBuGx3z4cvwU9MmrPZZ", Amount = 342});
            OperationClient.Add(new TransactionContent() {Destination = "edsk4ArLQgBTLWG5FJmnGnT689VKoqhXwmDPBuGx3z4cvwU9MmrPZZ", Amount = 11});
            var result = await OperationClient.Fill.Sign.Inject.CallAsync();
            var hash = await HeaderClient.BakeBlock("bootstrap3").Fill().Work.Sign.InjectBlock.CallAsync();

            var balance = await Rpc.Blocks.Head.Context.Contracts["edsk4ArLQgBTLWG5FJmnGnT689VKoqhXwmDPBuGx3z4cvwU9MmrPZZ"].Balance.GetAsync<string>();
            Assert.Equal("100500000376", balance);
        }

        [Fact]
        public async Task ForceBakeBlock()
        {
            await HeaderClient.BakeBlock("bootstrap1").Fill().Work.Sign.InjectBlock.CallAsync();
            var pendingOp = await Rpc.Mempool.PendingOperations.GetAsync<MempoolOperations>();
            Assert.Single(pendingOp.Applied);
        }



    }
}