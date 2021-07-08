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

        public HeaderClient(TezosRpc rpc, string protocol, string key, string blockId)
        {
            Values = new RequiredValues()
            {
                ProtocolHash = protocol,
                Key = key,
                BlockId = blockId
            };
            Rpc = rpc;
        }
    }
}