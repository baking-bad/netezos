using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            KeyStore keyStore,
            ProtocolParametersContent parameters = null,
            SandboxConstants constants = null,
            string signature = null)
        {
            BlockHeaderClient = new BlockHeaderClient(rpc, protocolHash, keyStore, parameters, signature);
            BlockOperationsClient = new BlockOperationsClient(rpc, constants);
        }

        public IBlockOperationsClient BlockOperationGroup(Key key, List<OperationContent> operations)
        {
            BlockOperationsClient.SetOperationGroup(key, operations);
            return BlockOperationsClient;
        }

        public IBlockHeaderClient Header => BlockHeaderClient;

        public async Task<dynamic> ActivateProtocol(string keyName, string blockId) => 
            await BlockHeaderClient.ActivateProtocol(keyName).Fill(blockId).Sign.InjectBlock.CallAsync();

        public async Task<dynamic> ActivateProtocol(Key key, string blockId) => 
            await BlockHeaderClient.ActivateProtocol(key).Fill(blockId).Sign.InjectBlock.CallAsync();

        public async Task<dynamic> BakeBlock(Key key, string blockId)
        {
            lock (this)
            {
                return BlockHeaderClient.BakeBlock(key).Fill(blockId).Work.Sign.InjectBlock.CallAsync().Result;
            }
        }

        public async Task<dynamic> BakeBlock(string keyName, string blockId)
        {
            lock (this)
            {
                return BlockHeaderClient.BakeBlock(keyName).Fill(blockId).Work.Sign.InjectBlock.CallAsync().Result;
            }
        }

        public void Dispose()
        {
            BlockHeaderClient?.Dispose();
            BlockOperationsClient?.Dispose();
        }
    }
}