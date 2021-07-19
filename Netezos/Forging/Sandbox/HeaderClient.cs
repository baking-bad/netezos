using System.Collections.Generic;
using Netezos.Forging.Sandbox.Operations;
using Netezos.Rpc;

namespace Netezos.Forging.Sandbox.Header
{
    public partial class HeaderClient
    {
        private readonly TezosRpc Rpc;
        private readonly HeaderParameters Values;

        public ActivateProtocolOperation ActivateProtocol(string keyName) => new ActivateProtocolOperation(Rpc, Values, keyName);
        public BakeBlockOperation BakeBlock(string keyName, int minFee = 0) => new BakeBlockOperation(Rpc, Values, keyName, minFee);

        /// <summary>
        /// Client for block creation call 
        /// </summary>
        /// <param name="rpc">Rpc client</param>
        /// <param name="protocolHash">Protocol hash(required)</param>
        /// <param name="keys"></param>
        /// <param name="protocolParameters">Protocol parameters for sandbox node(activate protocol)</param>
        /// <param name="signature">signature(optional)</param>
        public HeaderClient(TezosRpc rpc, string protocolHash, Dictionary<string, string> keys, dynamic protocolParameters, string signature = null)
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
    }
}