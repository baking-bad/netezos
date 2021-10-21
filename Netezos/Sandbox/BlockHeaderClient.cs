using System;
using Netezos.Forging.Models;
using Netezos.Keys;
using Netezos.Rpc;
using Netezos.Sandbox.HeaderMethods;
using Netezos.Sandbox.Models;

namespace Netezos.Sandbox
{
    internal class BlockHeaderClient : IBlockHeaderClient, IDisposable
    {
        private readonly TezosRpc Rpc;
        private readonly HeaderParameters Values;
        private readonly KeyStore KeyStore;

        /// <summary>
        /// Client for block creation call 
        /// </summary>
        /// <param name="rpc">Rpc client</param>
        /// <param name="protocolHash">Protocol hash(required)</param>
        /// <param name="keyStore">Store keys</param>
        /// <param name="protocolParameters">Protocol parameters for sandbox node(activate protocol)</param>
        /// <param name="signature">signature(optional)</param>
        internal BlockHeaderClient(TezosRpc rpc, string protocolHash, KeyStore keyStore, ProtocolParametersContent protocolParameters, string signature = null)
        {
            Values = new HeaderParameters()
            {
                ProtocolHash = protocolHash,
                ProtocolParameters = protocolParameters,
                Signature = signature
            };
            KeyStore = keyStore;
            Rpc = rpc;
        }

        public ActivateProtocolMethodHandler ActivateProtocol(string aliasKeyOrPk, string protocolHash = null)
            => ActivateProtocol(KeyStore[aliasKeyOrPk], protocolHash);

        public ActivateProtocolMethodHandler ActivateProtocol(Key key, string protocolHash = null)
        {
            var headerParameters = Values.Copy();
            headerParameters.Key = key;
            if (protocolHash != null)
                headerParameters.ProtocolHash = protocolHash;
            return new ActivateProtocolMethodHandler(Rpc, headerParameters);
        }

        public BakeBlockMethodHandler BakeBlock(string aliasKeyOrPk, int minFee = 0, string protocolHash = null) =>
            BakeBlock(KeyStore[aliasKeyOrPk], minFee, protocolHash);

        public BakeBlockMethodHandler BakeBlock(Key key, int minFee = 0, string protocolHash = null)
        {
            var headerParameters = Values.Copy();
            headerParameters.Key = key;
            headerParameters.MinFee = minFee;
            if (protocolHash != null)
                headerParameters.ProtocolHash = protocolHash;
            return new BakeBlockMethodHandler(Rpc, headerParameters);
        }

        public void Dispose() => Rpc.Dispose();
    }
}