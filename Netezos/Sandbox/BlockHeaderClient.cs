using System;
using System.Collections.Generic;
using Netezos.Forging.Models;
using Netezos.Rpc;
using Netezos.Sandbox.Base;
using Netezos.Sandbox.HeaderMethods;
using Netezos.Sandbox.Models;

namespace Netezos.Sandbox
{
    public class BlockHeaderClient : IBlockHeaderClient, IDisposable
    {
        private readonly TezosRpc Rpc;
        private readonly HeaderParameters Values;

        /// <summary>
        /// Client for block creation call 
        /// </summary>
        /// <param name="rpc">Rpc client</param>
        /// <param name="protocolHash">Protocol hash(required)</param>
        /// <param name="keys">dictionary keys(key is alias, value is key)</param>
        /// <param name="protocolParameters">Protocol parameters for sandbox node(activate protocol)</param>
        /// <param name="signature">signature(optional)</param>
        public BlockHeaderClient(TezosRpc rpc, string protocolHash, Dictionary<string, string> keys, ProtocolParametersContent protocolParameters, string signature = null)
        {
            Values = new HeaderParameters()
            {
                ProtocolHash = protocolHash,
                Keys = keys,
                ProtocolParameters = protocolParameters,
                Signature = signature
            };
            Rpc = rpc;
        }

        public ActivateProtocolMethodHandler ActivateProtocol(string keyName) => new ActivateProtocolMethodHandler(Rpc, Values, keyName);

        public BakeBlockMethodHandler BakeBlock(string keyName, int minFee = 0) => new BakeBlockMethodHandler(Rpc, Values, keyName, minFee);

        public void Dispose() => Rpc.Dispose();
    }
}