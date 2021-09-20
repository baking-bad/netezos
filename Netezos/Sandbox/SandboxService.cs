using System;
using System.Collections.Generic;
using Netezos.Forging.Models;
using Netezos.Keys;
using Netezos.Rpc;
using Netezos.Sandbox.Models;

namespace Netezos.Sandbox
{
    public class SandboxService : IDisposable
    {
        private BlockHeaderClient BlockHeaderClient;
        private BlockOperationsClient BlockOperationsClient;

        public SandboxService(
            TezosRpc rpc,
            string protocolHash,
            Dictionary<string, string> keys,
            ProtocolParametersContent parameters = null,
            SandboxConstants constants = null,
            string signature = null)
        {
            BlockHeaderClient = new BlockHeaderClient(rpc, protocolHash, keys, parameters, signature);
            BlockOperationsClient = new BlockOperationsClient(rpc, constants);
        }

        public IBlockOperationsClient BlockOperationGroup(Key key, List<OperationContent> operations)
        {
            BlockOperationsClient.SetOperationGroup(key, operations);
            return BlockOperationsClient;
        }

        public IBlockHeaderClient Header => BlockHeaderClient;

        public void Dispose()
        {
            BlockHeaderClient?.Dispose();
            BlockOperationsClient?.Dispose();
        }
    }
}