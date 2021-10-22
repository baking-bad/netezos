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

        /*[Fact, Order(4)]
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
        }*/

        [Fact, Order(5)]
        public async Task TestRevealPublicKeyHashSendTez()
        {
            await Task.Delay(TimeSpan.FromSeconds(5));

            var operationGroup = new List<OperationContent>()
            {
                // new RevealContent(),
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

            var result = await SandboxService.BlockOperationGroup(Key.FromBase58("edsk39qAm1fiMjgmPkw1EgQYkMzkJezLNewd7PLNHTkr6w9XA2zdfo"), operationGroup).Fill().Sign.Inject.CallAsync();
            var hash = await SandboxService.BakeBlock("bootstrap1", "head");

            // var balance = await Rpc.Blocks.Head.Context.Contracts["edsk4ArLQgBTLWG5FJmnGnT689VKoqhXwmDPBuGx3z4cvwU9MmrPZZ"].Balance.GetAsync<string>();
            // Assert.Equal("100500000376", balance);
        }

        /*[Fact, Order(10)]
        public async Task TestBakeEmptyBlock()
        {
            await Task.Delay(TimeSpan.FromSeconds(5));

            await SandboxService.BakeBlock("bootstrap1", "head");
            var pendingOp = await Rpc.Mempool.PendingOperations.GetAsync<MempoolOperations>();
            Assert.Empty(pendingOp.Applied);
        }*/

        /*[Fact, Order(8)]
        public async Task TestRollback()
        {
            await Task.Delay(TimeSpan.FromSeconds(5));

            await SandboxService
                .Header
                .ActivateProtocol("dictator", "PtEdo2ZkT9oKpimTah6x2embF25oss54njMuPzkJTEi5RqfdZFA")
                .Fill()
                .Sign
                .InjectBlock
                .CallAsync();

            var balance = await Rpc.Blocks.Head.Context.Contracts["edsk3gUfUPyBSfrS9CCgmCiQsTCHGkviBDusMxDJstFtojtc1zcpsh"].Balance.GetAsync<string>();
            Assert.Equal("4000000000000", balance);

        }*/
    }
}