using System;
using System.Collections.Generic;
using Netezos.Forging.Models;
using Netezos.Keys;
using Netezos.Rpc;
using Netezos.Sandbox.BlockMethods;
using Netezos.Sandbox.Models;

namespace Netezos.Sandbox
{
    internal class BlockOperationsClient : IBlockOperationsClient, IDisposable
    {
        private readonly TezosRpc Rpc;
        private BlockParameters Values;

        internal BlockOperationsClient(
            TezosRpc rpc,
            SandboxConstants constants)
        {
            Values = new BlockParameters()
            {
                Constants = constants ?? new SandboxConstants(),
                IsSandbox = true
            };
            Rpc = rpc;
        }

        internal void SetOperationGroup(Key key, List<OperationContent> operations)
        {
            Values.Key = key;
            Values.Operations = operations;
        }

        public FillMethodHandler Fill => new FillMethodHandler(Rpc, Values);

        // public AutoFillMethodHandler AutoFill => new AutoFillMethodHandler(Rpc, Values);

        public void Add(OperationContent operation)
        {
            Values.Operations.Add(operation);
        }

        public void AddRange(List<OperationContent> operations)
        {
            Values.Operations.AddRange(operations);
        }

        public void Clear()
        {
            Values.Operations = new List<OperationContent>();
        }

        public void Dispose()
        {
            Rpc?.Dispose();
            Values?.Operations?.Clear();
        }

        FillMethodHandler IBlockOperationsClient.Fill() => Fill;

        // AutoFillMethodHandler IBlockOperationsClient.AutoFill() => AutoFill;
    }
}