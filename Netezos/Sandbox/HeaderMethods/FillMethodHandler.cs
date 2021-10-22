using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Netezos.Encoding;
using Netezos.Forging.Models;
using Netezos.Keys;
using Netezos.Rpc;
using Netezos.Sandbox.Models;

namespace Netezos.Sandbox.HeaderMethods
{
    /// <summary>
    /// Fill missing fields essential for preapply 
    /// </summary>
    public class FillMethodHandler : HeaderMethodHandler
    {
        private readonly bool FromBakeCall;
        private readonly string BlockId;

        public SignMethodHandler Sign => new SignMethodHandler(Rpc, Values, CallAsync);

        public WorkMethodHandler Work => new WorkMethodHandler(Rpc, Values, CallAsync);


        internal FillMethodHandler(
            TezosRpc rpc,
            HeaderParameters parameters,
            Func<HeaderParameters, Task<ForwardingParameters>> function,
            string blockId,
            bool fromBakeBlock = false) : base(rpc, parameters, function)
        {
            BlockId = blockId;
            FromBakeCall = fromBakeBlock;
        }

        public override async Task<dynamic> CallAsync() => await CallAsync(Values);

        internal override async Task<ForwardingParameters> CallAsync(HeaderParameters data)
        {
            var parameters = await Function(data);
            var header = parameters.BlockHeader;

            var predShellHeader = await Rpc.Blocks[BlockId].Header.Shell.GetAsync<ShellHeaderContent>();
            var timestamp = predShellHeader.Timestamp + TimeSpan.FromSeconds(1);

            var protocols = await Rpc.Blocks[BlockId].Protocols.GetAsync<Dictionary<string, string>>();
            header.ProtocolData.ProtocolHash = protocols["next_protocol"];

            parameters.Operations?
                .ForEach(x => 
                    x.ForEach(h => h.Protocol = header.ProtocolData.ProtocolHash));

            parameters.Signature = new Signature(new byte[64], Prefix.sig);

            FillSeedNonceHash(
                header.ProtocolData,
                (int)data.ProtocolParameters.BlocksPerCommitment,
                predShellHeader.Level);

            if (FromBakeCall)
            {
                await FillPriority(header.ProtocolData, data.Key.GetBase58(), BlockId);

                var bakeBlockResult = await Rpc
                    .Blocks
                    .Head
                    .Helpers
                    .Preapply
                    .Block
                    .PostAsync<PreapplyShellHeaderResult>(
                        header.ProtocolData.ProtocolHash,
                        header.ProtocolData.Priority,
                        header.ProtocolData.ProofOfWorkNonce,
                        parameters.Signature,
                        parameters.Operations?.Select(x => x.Select(y => (object)y))
                        ?? new List<List<object>>()
                    );

                parameters.BlockHeader.ShellHeader = bakeBlockResult.ShellHeader;

                foreach (var operation in bakeBlockResult.Operations)
                {
                    parameters.ForgedOperations.Add(operation["applied"]);
                }

                return parameters;
            }

            if (header.ProtocolData is ActivationProtocolDataContent protocolData)
            {
                var result = await Rpc
                    .Blocks
                    .Head
                    .Helpers
                    .Preapply
                    .Block
                    .PostAsync<PreapplyShellHeaderResult>(
                        header.ProtocolData.ProtocolHash,
                        protocolData.Content.Command,
                        protocolData.Content.Hash,
                        protocolData.Content.Fitness,
                        protocolData.Content.ProtocolParameters,
                        parameters.Signature,
                        parameters.Operations?.Select(x => x.Select(y => (object)y))
                        ?? new List<List<object>>(),
                        timestamp,
                        true);

                parameters.BlockHeader.ShellHeader = result.ShellHeader;
            }

            return parameters;
        }

        private void FillSeedNonceHash(ProtocolDataContent protocolData, int blocksPerCommitment, int level)
            => protocolData.SeedNonceHash = (level+1) % blocksPerCommitment == 0
                ? Base58.Convert(new byte[32], Prefix.nce)
                : string.Empty;

        private async Task FillPriority(ProtocolDataContent protocolData, string key, string blockId)
        {
            var baker = Key.FromBase58(key).PubKey.Address;
            var bakingRights = await Rpc.Blocks[blockId].Helpers.BakingRights.GetAsync<List<Dictionary<string, object>>>(baker);

            var bakingRightByBaker = bakingRights
                .FirstOrDefault(b => b.TryGetValue("delegate", out var value) && value.ToString().Equals(baker));

            if (bakingRightByBaker == null)
                throw new InvalidOperationException($"Not found baking right with baker {baker}");
            
            protocolData.Priority = bakingRightByBaker.TryGetValue("priority", out var priority) 
                ? ((JsonElement)priority).GetInt32()
                : throw new KeyNotFoundException($"Not found priority field into baking right with baker {baker}");
            ;
        }
    }
}