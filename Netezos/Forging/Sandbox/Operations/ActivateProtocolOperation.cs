using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Netezos.Encoding;
using Netezos.Forging.Models;
using Netezos.Keys;
using Netezos.Rpc;

namespace Netezos.Forging.Sandbox.Base
{
    public class ActivateProtocolOperation : HeaderOperation
    {
        public FillOperation Fill => new FillOperation(Rpc, Values, CallAsync);
        
        internal ActivateProtocolOperation(TezosRpc rpc, HeaderParameters headerParameters) : base(rpc, headerParameters) { }
        
        public override async Task<dynamic> CallAsync() => await CallAsync(Values);

        protected override async Task<(ShellHeaderContent, BlockHeaderContent, Signature)> CallAsync(HeaderParameters data)
        {
            var header = await Rpc.Blocks.Head.Header.Shell.GetAsync<ShellHeaderContent>();
            var fitness = BumpFitness(header.Fitness);

            var blockHeader = new BlockHeaderContent()
            {
                ProtocolData = new ActivationProtocolDataContent()
                {
                    Content = new ActivationCommandContent()
                    {
                        Hash = data.ProtocolHash,
                        Fitness = fitness,
                        ProtocolParameters = data.ProtocolParameters
                    }
                }
            };
            return (null, blockHeader, null);
        }

        private List<string> BumpFitness(List<string> fitness)
        {
            var major = int.Parse(fitness?.FirstOrDefault() ?? "0", System.Globalization.NumberStyles.HexNumber);
            var minor = long.Parse(fitness?.LastOrDefault() ?? "0", System.Globalization.NumberStyles.HexNumber) + 1;
            return new List<string>()
            {
                Hex.Convert(LocalForge.ForgeInt32(major, 1)),
                Hex.Convert(LocalForge.ForgeInt64(minor, 8))
            };
        } 
    }
}