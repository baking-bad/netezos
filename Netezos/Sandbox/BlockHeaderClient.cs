using System;
using System.Collections.Generic;
using Netezos.Forging.Models;
using Netezos.Keys;
using Netezos.Rpc;
using Netezos.Sandbox.Base;
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

        public ActivateProtocolMethodHandler ActivateProtocol(string aliasKeyOrPk)
            => ActivateProtocol(KeyStore[aliasKeyOrPk]);

        public ActivateProtocolMethodHandler ActivateProtocol(Key key)
        {
            var headerParameters = Values.Copy();
            headerParameters.Key = key;
            return new ActivateProtocolMethodHandler(Rpc, headerParameters);
        }

        public BakeBlockMethodHandler BakeBlock(string aliasKeyOrPk, int minFee = 0) =>
            BakeBlock(KeyStore[aliasKeyOrPk], minFee);

        public BakeBlockMethodHandler BakeBlock(Key key, int minFee = 0)
        {
            var headerParameters = Values.Copy();
            headerParameters.Key = key;
            headerParameters.MinFee = minFee;
            return new BakeBlockMethodHandler(Rpc, headerParameters);
        }

        public void Dispose() => Rpc.Dispose();
    }
}