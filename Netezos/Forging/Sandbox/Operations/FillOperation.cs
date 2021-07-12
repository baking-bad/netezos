using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Netezos.Forging.Models;
using Netezos.Keys;
using Netezos.Rpc;
using Org.BouncyCastle.Math;

namespace Netezos.Forging.Sandbox.Base
{
    /// <summary>
    /// Fill missing fields essential for preapply 
    /// </summary>
    public class FillOperation : HeaderOperation
    {
        public SignOperation Sign => new SignOperation(Rpc, Values, CallAsync);
        
        internal FillOperation(
            TezosRpc rpc, 
            HeaderParameters headerParameters, 
            Func<HeaderParameters, Task<(ShellHeaderContent, BlockHeaderContent, Signature)>> function) 
            : base(rpc, headerParameters, function) { }

        public override async Task<dynamic> CallAsync() => await CallAsync(Values);

        protected override async Task<(ShellHeaderContent, BlockHeaderContent, Signature)> CallAsync(HeaderParameters data)
        {
            var (_, header, _) = await Function(data);

            var predShellHeader = await Rpc.Blocks[data.BlockId].Header.Shell.GetAsync<ShellHeaderContent>();
            var timestamp = predShellHeader.Timestamp + TimeSpan.FromSeconds(1);

            var protocols = await Rpc.Blocks[data.BlockId].Protocols.GetAsync<IDictionary<string, string>>();
            header.ProtocolData.ProtocolHash = protocols["next_protocol"];

            var key = BigInteger.Zero.ToByteArrayUnsigned().Align(64);
            var dummySignature = new Signature(key, Prefix.sig);
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
                    dummySignature.ToBase58(),
                    new List<List<object>>(), 
                    timestamp,
                    true);

            return (result.ShellHeader, header, dummySignature);

        } 
    }
}