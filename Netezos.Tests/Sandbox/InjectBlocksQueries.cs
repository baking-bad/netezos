using System.Collections.Generic;
using System.Threading.Tasks;
using Netezos.Forging.Models;
using Netezos.Keys;
using Netezos.Rpc;
using Netezos.Sandbox;
using Netezos.Tests.Rpc;
using Xunit;

namespace Netezos.Tests.Sandbox
{
    [Collection(SettingsCollection.CollectionName)]
    public class InjectBlocksQueries
    {
        readonly TezosRpc Rpc;
        readonly SandboxService SandboxService;
        readonly Key Key;
        
        public InjectBlocksQueries(SettingsFixture settings)
        {
            SandboxService = settings.SandboxService;
            Rpc = settings.Rpc;
            Key = settings.ActiveKey;
        }

        [Fact]
        public async Task TestActivateProtocol()
        {
            var operationGroup = new List<OperationContent>()
            {
                new ActivationContent()
            };

            await SandboxService.BlockOperationGroup(Key, operationGroup).Fill().Sign.Inject.CallAsync();
            var hash = await SandboxService.Header.BakeBlock("bootstrap1").Fill().Work.Sign.InjectBlock.CallAsync();

            var balance = await Rpc.Blocks.Head.Context.Contracts["tz1W86h1XuWy6awbNUTRUgs6nk8q5vqXQwgk"].Balance.GetAsync<string>();
            Assert.Equal("100500000000", balance);
        }

        [Fact]
        public async Task RevealPublicKeyHashSendTez()
        {
            var operationGroup = new List<OperationContent>()
            {
                new RevealContent(),
                new TransactionContent(){Destination = "edsk3gUfUPyBSfrS9CCgmCiQsTCHGkviBDusMxDJstFtojtc1zcpsh", Amount = 23}
            };

            var result = await SandboxService.BlockOperationGroup(Key, operationGroup).Fill().Sign.Inject.CallAsync();
            var hash = await SandboxService.Header.BakeBlock("bootstrap1").Fill().Work.Sign.InjectBlock.CallAsync();

            var balance = await Rpc.Blocks.Head.Context.Contracts["edsk3gUfUPyBSfrS9CCgmCiQsTCHGkviBDusMxDJstFtojtc1zcpsh"].Balance.GetAsync<string>();
            Assert.Equal("4000000000023", balance);
        }

        [Fact]
        public async Task SendMultipleTransactions()
        {
            var operationGroup = new List<OperationContent>()
            {
                new TransactionContent()
                    {Destination = "edsk4ArLQgBTLWG5FJmnGnT689VKoqhXwmDPBuGx3z4cvwU9MmrPZZ", Amount = 23},
                new TransactionContent()
                    {Destination = "edsk4ArLQgBTLWG5FJmnGnT689VKoqhXwmDPBuGx3z4cvwU9MmrPZZ", Amount = 342},
                new TransactionContent()
                    {Destination = "edsk4ArLQgBTLWG5FJmnGnT689VKoqhXwmDPBuGx3z4cvwU9MmrPZZ", Amount = 11},

            };

            var result = await SandboxService.BlockOperationGroup(Key, operationGroup).Fill().Sign.Inject.CallAsync();
            var hash = await SandboxService.Header.BakeBlock("bootstrap1").Fill().Work.Sign.InjectBlock.CallAsync();

            var balance = await Rpc.Blocks.Head.Context.Contracts["edsk4ArLQgBTLWG5FJmnGnT689VKoqhXwmDPBuGx3z4cvwU9MmrPZZ"].Balance.GetAsync<string>();
            Assert.Equal("100500000376", balance);
        }

        [Fact]
        public async Task ForceBakeBlock()
        {
            await SandboxService.Header.BakeBlock("bootstrap1").Fill().Work.Sign.InjectBlock.CallAsync();
            var pendingOp = await Rpc.Mempool.PendingOperations.GetAsync<MempoolOperations>();
            Assert.Single(pendingOp.Applied);
        }



    }
}