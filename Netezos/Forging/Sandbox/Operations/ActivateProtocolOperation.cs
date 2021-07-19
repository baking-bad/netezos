using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Netezos.Encoding;
using Netezos.Forging.Models;
using Netezos.Rpc;
using Netezos.Utils.Json;

namespace Netezos.Forging.Sandbox.Operations
{
    public class ActivateProtocolOperation : HeaderOperation
    {
        public FillOperation Fill(string blockId = "genesis") => new FillOperation(Rpc, Values, CallAsync, blockId);

        internal ActivateProtocolOperation(TezosRpc rpc, HeaderParameters headerParameters, string keyName) 
            : base(rpc, headerParameters)
        {
            headerParameters.Key = headerParameters.Keys.TryGetValue(keyName, out var key) 
                ? key
                : throw new KeyNotFoundException($"Parameter keyName {keyName} is not found");
        }
        
        public override async Task<dynamic> CallAsync() => await CallAsync(Values);

        internal override async Task<ForwardingParameters> CallAsync(HeaderParameters data)
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
                        ProtocolParameters = Hex.Convert(LocalForge.ForgeArray(
                            BsonSerializer.Serialize(data.ProtocolParameters.ToString())))
                    }
                }
            };
            return new ForwardingParameters
            {
                BlockHeader = blockHeader
            };
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