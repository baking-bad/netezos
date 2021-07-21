using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Netezos.Encoding;
using Netezos.Forging.Models;
using Netezos.Keys;
using Netezos.Rpc;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Math;

namespace Netezos.Forging.Sandbox.Operations
{
    /// <summary>
    /// Fill missing fields essential for preapply 
    /// </summary>
    public class FillOperation : HeaderOperation
    {
        private readonly bool _fromBakeBlockCall;

        public SignOperation Sign => new SignOperation(Rpc, Values, CallAsync);

        public WorkOperation Work => new WorkOperation(Rpc, Values, CallAsync);

        private readonly string _blockId;

        internal FillOperation(
            TezosRpc rpc,
            HeaderParameters headerParameters,
            Func<HeaderParameters, Task<ForwardingParameters>> function,
            string blockId,
            bool fromBakeBlock = false) : base(rpc, headerParameters, function)
        {
            _blockId = blockId;
            _fromBakeBlockCall = fromBakeBlock;
        }

        public override async Task<dynamic> CallAsync() => await CallAsync(Values);

        internal override async Task<ForwardingParameters> CallAsync(HeaderParameters data)
        {
            var parameters = await Function(data);
            var header = parameters.BlockHeader;

            var predShellHeader = await Rpc.Blocks[_blockId].Header.Shell.GetAsync<ShellHeaderContent>();
            var timestamp = predShellHeader.Timestamp + TimeSpan.FromSeconds(1);

            var protocols = await Rpc.Blocks[_blockId].Protocols.GetAsync<Dictionary<string, string>>();
            header.ProtocolData.ProtocolHash = protocols["next_protocol"];
            parameters.Operations?
                .ForEach(x => 
                    x.ForEach(h => h.Protocol = protocols["next_protocol"]));

            var bytes = BigInteger.Zero.ToByteArrayUnsigned().Align(64);
            parameters.Signature = new Signature(bytes, Prefix.sig);

            FillSeedNonceHash(
                header.ProtocolData,
                (int)data.ProtocolParameters.BlocksPerCommitment,
                predShellHeader.Level);

            if (_fromBakeBlockCall)
            {
                await FillPriority(header.ProtocolData, data.Key, _blockId);
                var bakeBlockResult = await Rpc
                    .Blocks
                    .Head
                    .Helpers
                    .Preapply
                    .Block
                    .PostAsync<ShellHeaderWithOperations>(
                        header.ProtocolData.ProtocolHash,
                        header.ProtocolData.Priority,
                        header.ProtocolData.ProofOfWorkNonce,
                        parameters.Signature.ToBase58(),
                        parameters.Operations?.Select(x => x.Select(y => (object)y))
                                ?? new List<List<object>>()
                        );
                parameters.ShellHeader = bakeBlockResult.ShellHeader;
                return parameters;

            }

            var result = await Rpc
                .Blocks
                .Head
                .Helpers
                .Preapply
                .Block
                .PostAsync<ShellHeaderWithOperations>(
                    header.ProtocolData.ProtocolHash,
                    header.ProtocolData.Content.Command,
                    header.ProtocolData.Content.Hash,
                    header.ProtocolData.Content.Fitness,
                    header.ProtocolData.Content.ProtocolParameters,
                    parameters.Signature.ToBase58(),
                    parameters.Operations?.Select(x => x.Select(y => (object)y))
                            ?? new List<List<object>>(),
                    timestamp,
                    true);

            parameters.ShellHeader = result.ShellHeader;
            return parameters;
        }

        private void FillSeedNonceHash(ProtocolDataContent protocolData, int blocksPerCommitment, int level)
            => protocolData.SeedNonceHash = level % blocksPerCommitment == 0
                ? Base58.Convert(BigInteger.Zero.ToByteArrayUnsigned().Align(64), Prefix.nce)
                : string.Empty;

        private async Task FillPriority(ProtocolDataContent protocolData, string key, string blockId)
        {
            var baker = Key.FromBase58(key).PubKey.Address;
            var bakingRights = await Rpc.Blocks[blockId].Helpers.BakingRights.GetAsync<List<Dictionary<string, dynamic>>>(baker);
            var item = bakingRights.First(b => b["delegate"].ToString() == baker)["priority"];
            protocolData.Priority = int.Parse(item.ToString());
        }
    }
}