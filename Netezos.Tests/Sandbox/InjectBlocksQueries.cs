using System.Collections.Generic;
using System.Threading.Tasks;
using Netezos.Forging.Models;
using Netezos.Keys;
using Netezos.Rpc;
using Netezos.Sandbox;
using Netezos.Tests.Startup;
using Xunit;
using Xunit.Extensions.Ordering;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
[assembly: TestCaseOrderer("Xunit.Extensions.Ordering.TestCaseOrderer", "Xunit.Extensions.Ordering")]
[assembly: TestCollectionOrderer("Xunit.Extensions.Ordering.CollectionOrderer", "Xunit.Extensions.Ordering")]
namespace Netezos.Tests.Sandbox
{
    [Collection(SettingsCollection.CollectionName)]
    public class InjectBlocksQueries
    {
        readonly TezosRpc Rpc;
        readonly SandboxService SandboxService;
        readonly CommitmentKey ActiveKey;

        public InjectBlocksQueries(SettingsFixture settings)
        {
            SandboxService = settings.SandboxService;
            Rpc = settings.Rpc;
            ActiveKey = settings.KeyStore.ActivatorKey;
        }

        [Fact, Order(1)]
        public async Task TestActivateProtocol()
        {
            var header = await Rpc.Blocks.Head.Header.Shell.GetAsync<ShellHeaderContent>();
            Assert.NotNull(header.Context);
        }

        [Fact, Order(2)]
        public async Task TestActivateAccount()
        {
            var operationGroup = new List<OperationContent>()
            {
                new ActivationContent() { }
            };

            await SandboxService.BlockOperationGroup(ActiveKey, operationGroup).Fill().Sign.Inject.CallAsync();
            var hash = await SandboxService.BakeBlock("bootstrap1", "head");

            var balance = await Rpc.Blocks.Head.Context.Contracts["tz1W86h1XuWy6awbNUTRUgs6nk8q5vqXQwgk"].Balance.GetAsync<string>();
            Assert.Equal("100500000000", balance);
        }

        [Fact, Order(3)]
        public async Task TestReveal()
        {
            var operationGroup = new List<OperationContent>()
            {
                new RevealContent(),
            };
            var result = await SandboxService.BlockOperationGroup(ActiveKey, operationGroup).Fill().Sign.Inject.CallAsync();
            var hash = await SandboxService.BakeBlock("bootstrap1", "head");

            Assert.NotNull(hash);
        }

        [Fact, Order(4)]
        public async Task TestRevealPublicKeyHashSendTez()
        {
            var operationGroup = new List<OperationContent>()
            {
                // new RevealContent(),
                new TransactionContent(){ Destination = "tz1faswCTDciRzE4oJ9jn2Vm2dvjeyA9fUzU", Amount = 23 }
            };

            var result = await SandboxService.BlockOperationGroup(Key.FromBase58("edsk39qAm1fiMjgmPkw1EgQYkMzkJezLNewd7PLNHTkr6w9XA2zdfo"), operationGroup).Fill().Sign.Inject.CallAsync();
            var hash = await SandboxService.BakeBlock("bootstrap1", "head");

            var balance = await Rpc.Blocks.Head.Context.Contracts["edsk3gUfUPyBSfrS9CCgmCiQsTCHGkviBDusMxDJstFtojtc1zcpsh"].Balance.GetAsync<string>();
            Assert.Equal("4000000000023", balance);
        }

        [Fact, Order(5)]
        public async Task TestSendMultipleTransactions()
        {
            var operationGroup = new List<OperationContent>()
            {
                new TransactionContent()
                    {Destination = "tz1gjaF81ZRRvdzjobyfVNsAeSC6PScjfQwN", Amount = 23},
                new TransactionContent()
                    {Destination = "tz1faswCTDciRzE4oJ9jn2Vm2dvjeyA9fUzU", Amount = 342},
                new TransactionContent()
                    {Destination = "tz1b7tUupMgCNw2cCLpKTkSD1NZzB5TkP2sv", Amount = 11},

            };

            var result = await SandboxService.BlockOperationGroup(Key.FromBase58("edsk39qAm1fiMjgmPkw1EgQYkMzkJezLNewd7PLNHTkr6w9XA2zdfo"), operationGroup).Fill().Sign.Inject.CallAsync();
            var hash = await SandboxService.BakeBlock("bootstrap1", "head");


            // var balance = await Rpc.Blocks.Head.Context.Contracts["edsk4ArLQgBTLWG5FJmnGnT689VKoqhXwmDPBuGx3z4cvwU9MmrPZZ"].Balance.GetAsync<string>();
            // Assert.Equal("100500000376", balance);
        }

        [Fact, Order(6)]
        public async Task TestForceBakeBlock()
        {
            var hash = await SandboxService.BakeBlock("bootstrap1", "head");
            var pendingOp = await Rpc.Mempool.PendingOperations.GetAsync<MempoolOperations>();
            Assert.Empty(pendingOp.Applied);
        }
    }
}