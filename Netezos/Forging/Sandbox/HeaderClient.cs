using System;
using System.Collections.Generic;
using Netezos.Forging.Models;
using Netezos.Forging.Sandbox.Operations;
using Netezos.Rpc;

namespace Netezos.Forging.Sandbox.Header
{
    public partial class HeaderClient : IDisposable
    {
        private readonly TezosRpc Rpc;
        private readonly HeaderParameters Values;

        /// <summary>
        /// Create call to bake genesis block with specified parameters
        /// </summary>
        /// <param name="keyName">alias key</param>
        /// <returns></returns>
        public ActivateProtocolOperation ActivateProtocol(string keyName) => new ActivateProtocolOperation(Rpc, Values, keyName);

        /// <summary>
        /// Create call to bake new block
        /// <param name="keyName">alias key</param>
        /// <param name="minFee">min fee</param>
        /// </summary>
        public BakeBlockOperation BakeBlock(string keyName, int minFee = 0) => new BakeBlockOperation(Rpc, Values, keyName, minFee);

        /// <summary>
        /// Client for block creation call 
        /// </summary>
        /// <param name="rpc">Rpc client</param>
        /// <param name="protocolHash">Protocol hash(required)</param>
        /// <param name="keys">dictionary keys(key is alias, value is key)</param>
        /// <param name="protocolParameters">Protocol parameters for sandbox node(activate protocol)</param>
        /// <param name="signature">signature(optional)</param>
        public HeaderClient(TezosRpc rpc, string protocolHash, Dictionary<string, string> keys, ProtocolParametersContent protocolParameters, string signature = null)
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

        public void Dispose() => Rpc.Dispose();
    }
}