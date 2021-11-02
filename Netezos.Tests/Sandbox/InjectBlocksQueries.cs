using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Netezos.Forging.Models;
using Netezos.Keys;
using Netezos.Rpc;
using Netezos.Sandbox;
using Netezos.Tests.Startup;
using Xunit;
using Xunit.Extensions.Ordering;

namespace Netezos.Tests.Sandbox
{
    [Collection(SettingsCollection.CollectionName)]
    [Order(1)]
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
            await Task.Delay(TimeSpan.FromSeconds(5));
            await SandboxService.ActivateProtocol("dictator", "genesis");
            var header = await Rpc.Blocks.Head.Header.Shell.GetAsync<ShellHeaderContent>();
            Assert.NotNull(header.Context);
        }

        [Fact, Order(2)]
        public async Task TestFirstBakeBlock()
        {
            await Task.Delay(TimeSpan.FromSeconds(5));
            await SandboxService.BakeBlock("bootstrap1", "head");
            var header = await Rpc.Blocks.Head.Header.Shell.GetAsync<ShellHeaderContent>();
            Assert.NotNull(header.Context);
        }

        [Fact, Order(3)]
        public async Task TestActivateAccount()
        {
            await Task.Delay(TimeSpan.FromSeconds(5));

            var operationGroup = new List<OperationContent>()
            {
                new ActivationContent() { }
            };

            await SandboxService.BlockOperationGroup(ActiveKey, operationGroup).Fill().Sign.Inject.CallAsync();
            var hash = await SandboxService.BakeBlock("bootstrap2", "head");

            var balance = await Rpc.Blocks.Head.Context.Contracts["tz1W86h1XuWy6awbNUTRUgs6nk8q5vqXQwgk"].Balance.GetAsync<string>();
            Assert.Equal("100500000000", balance);
        }

        [Fact, Order(4)]
        public async Task TestReveal()
        {
            await Task.Delay(TimeSpan.FromSeconds(5));

            var operationGroup = new List<OperationContent>()
            {
                new RevealContent(),
            };
            var result = await SandboxService.BlockOperationGroup(ActiveKey, operationGroup).Fill().Sign.Inject.CallAsync();
            var hash = await SandboxService.BakeBlock("bootstrap1", "head");

            Assert.NotNull(hash);
        }

        [Fact, Order(5)]
        public async Task TestRevealPublicKeyHashSendTez()
        {
            await Task.Delay(TimeSpan.FromSeconds(5));

            var operationGroup = new List<OperationContent>()
            {
                new TransactionContent(){ Destination = "tz1faswCTDciRzE4oJ9jn2Vm2dvjeyA9fUzU", Amount = 42 }
            };

            var key = Key.FromBase58("edsk3gUfUPyBSfrS9CCgmCiQsTCHGkviBDusMxDJstFtojtc1zcpsh");
            var result = await SandboxService
                .BlockOperationGroup(key, operationGroup)
                .Fill()
                .Sign
                .Inject
                .CallAsync();

            var hash = await SandboxService.BakeBlock("bootstrap1", "head");

            var balance = await Rpc.Blocks.Head.Context.Contracts["edsk3gUfUPyBSfrS9CCgmCiQsTCHGkviBDusMxDJstFtojtc1zcpsh"].Balance.GetAsync<string>();
            Assert.Equal("4000000000023", balance);
        }

        [Fact, Order(6)]
        public async Task TestSendMultipleTransactions()
        {
            await Task.Delay(TimeSpan.FromSeconds(5));

            var operationGroup = new List<OperationContent>()
            {
                new TransactionContent()
                    {Destination = "tz1gjaF81ZRRvdzjobyfVNsAeSC6PScjfQwN", Amount = 23},
                new TransactionContent()
                    {Destination = "tz1faswCTDciRzE4oJ9jn2Vm2dvjeyA9fUzU", Amount = 342},
                new TransactionContent()
                    {Destination = "tz1b7tUupMgCNw2cCLpKTkSD1NZzB5TkP2sv", Amount = 11},

            };

            var result = await SandboxService
                .BlockOperationGroup(Key.FromBase58("edsk39qAm1fiMjgmPkw1EgQYkMzkJezLNewd7PLNHTkr6w9XA2zdfo"), operationGroup)
                .Fill()
                .Sign
                .Inject
                .CallAsync();
            var hash = await SandboxService.BakeBlock("bootstrap2", "head");

            var balance1 = await Rpc.Blocks.Head.Context.Contracts["tz1gjaF81ZRRvdzjobyfVNsAeSC6PScjfQwN"].Balance.GetAsync<string>();
            var balance2 = await Rpc.Blocks.Head.Context.Contracts["tz1faswCTDciRzE4oJ9jn2Vm2dvjeyA9fUzU"].Balance.GetAsync<string>();
            var balance3 = await Rpc.Blocks.Head.Context.Contracts["tz1b7tUupMgCNw2cCLpKTkSD1NZzB5TkP2sv"].Balance.GetAsync<string>();

            Assert.Equal("3998975998426", balance1);
            Assert.Equal("4000000000384", balance2);
            Assert.Equal("4000000000011", balance3);
        }

        [Fact, Order(7)]
        public async Task TestBakeEmptyBlock()
        {
            await Task.Delay(TimeSpan.FromSeconds(5));

            await SandboxService.BakeBlock("bootstrap1", "head");
            var pendingOp = await Rpc.Mempool.PendingOperations.GetAsync<MempoolOperations>();
            Assert.Empty(pendingOp.Applied);
        }

         [Fact, Order(8)]
         public async Task TestAutoFillSendMultipleTransactions()
         {
             await Task.Delay(TimeSpan.FromSeconds(5));

             var operationGroup = new List<OperationContent>()
             {
                 new TransactionContent()
                     {Destination = "tz1gjaF81ZRRvdzjobyfVNsAeSC6PScjfQwN", Amount = 1},
                 new TransactionContent()
                     {Destination = "tz1faswCTDciRzE4oJ9jn2Vm2dvjeyA9fUzU", Amount = 123},
                 new TransactionContent()
                     {Destination = "tz1b7tUupMgCNw2cCLpKTkSD1NZzB5TkP2sv", Amount = 2},

             };

             var result = await SandboxService
                 .BlockOperationGroup(Key.FromBase58("edsk39qAm1fiMjgmPkw1EgQYkMzkJezLNewd7PLNHTkr6w9XA2zdfo"), operationGroup)
                 .AutoFill()
                 .Sign
                 .Inject
                 .CallAsync();
             var hash = await SandboxService.BakeBlock("bootstrap2", "head");

             var balance1 = await Rpc.Blocks.Head.Context.Contracts["tz1gjaF81ZRRvdzjobyfVNsAeSC6PScjfQwN"].Balance.GetAsync<string>();
             var balance2 = await Rpc.Blocks.Head.Context.Contracts["tz1faswCTDciRzE4oJ9jn2Vm2dvjeyA9fUzU"].Balance.GetAsync<string>();
             var balance3 = await Rpc.Blocks.Head.Context.Contracts["tz1b7tUupMgCNw2cCLpKTkSD1NZzB5TkP2sv"].Balance.GetAsync<string>();

             Assert.Equal("3998463997284", balance1);
             Assert.Equal("4000000000507", balance2);
             Assert.Equal("4000000000013", balance3);
         }

         [Fact, Order(9)]
         public async Task TestAutoFillSendSingleTransaction()
         {
             await Task.Delay(TimeSpan.FromSeconds(5));

             var operationGroup = new List<OperationContent>()
             {
                 new TransactionContent()
                     {Destination = "tz1gjaF81ZRRvdzjobyfVNsAeSC6PScjfQwN", Amount = 1},
             };

             var result = await SandboxService
                 .BlockOperationGroup(Key.FromBase58("edsk39qAm1fiMjgmPkw1EgQYkMzkJezLNewd7PLNHTkr6w9XA2zdfo"), operationGroup)
                 .AutoFill()
                 .Sign
                 .Inject
                 .CallAsync();
             var hash = await SandboxService.BakeBlock("bootstrap2", "head");

             var balance1 = await Rpc.Blocks.Head.Context.Contracts["tz1gjaF81ZRRvdzjobyfVNsAeSC6PScjfQwN"].Balance.GetAsync<string>();

             Assert.Equal("3997951996881", balance1);
         }
    }
}