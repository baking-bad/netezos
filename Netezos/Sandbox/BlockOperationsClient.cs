using System;
using System.Collections.Generic;
using Netezos.Forging.Models;
using Netezos.Keys;
using Netezos.Rpc;
using Netezos.Sandbox.Base;
using Netezos.Sandbox.BlockMethods;
using Netezos.Sandbox.Models;

namespace Netezos.Sandbox
{
    public class BlockOperationsClient : IDisposable
    {
        private readonly TezosRpc Rpc;
        private readonly BlockParameters Values;

        public BlockOperationsClient(TezosRpc rpc, Key key, List<OperationContent> operations = null)
        {
            Values = new BlockParameters()
            {
                Operations = new List<OperationContent>(),
                Key = key,
                IsSandbox = true
            };
            Rpc = rpc;
        }

        public FillMethodHandler Fill => new FillMethodHandler(Rpc, Values);

        public AutoFillMethodHandler AutoFill => new AutoFillMethodHandler(Rpc, Values);

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
    }
}