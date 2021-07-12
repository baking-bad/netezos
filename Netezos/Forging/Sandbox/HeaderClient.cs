using Netezos.Forging.Sandbox.Base;
using Netezos.Rpc;

namespace Netezos.Forging.Sandbox
{
    public class HeaderClient
    {
        private readonly TezosRpc Rpc;
        private readonly RequiredValues Values;

        public ActivateProtocolOperation ActivateProtocol => new ActivateProtocolOperation(Rpc, Values);
        public BakeBlockOperation BakeBlock => new BakeBlockOperation(Rpc, Values);

        /// <summary>
        /// Client for block creation call 
        /// </summary>
        /// <param name="rpc">Rpc client</param>
        /// <param name="protocolHash">Protocol hash(required)</param>
        /// <param name="key">Key(required)</param>
        /// <param name="blockId">blockId: head or genesis(required)</param>
        /// <param name="protocolParameters">Protocol parameters for sandbox node(activate protocol)</param>
        /// <param name="minFee">min fee for bakeCall(optional)</param>
        /// <param name="signature">signature(optional)</param>
        public HeaderClient(TezosRpc rpc, string protocolHash, string key, string blockId, string protocolParameters, int minFee = 0, string signature = null)
        {
            Values = new RequiredValues()
            {
                ProtocolHash = protocolHash,
                Key = key,
                ProtocolParameters = protocolParameters,
                BlockId = blockId,
                MinFee = minFee,
                Signature = signature
            };
            Rpc = rpc;
        }
    }
}