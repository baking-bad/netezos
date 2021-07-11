using System;
using System.Threading.Tasks;
using Netezos.Encoding;
using Netezos.Forging.Models;
using Netezos.Keys;
using Netezos.Rpc;

namespace Netezos.Forging.Sandbox.Base
{
    /// <summary>
    /// Sign the block header with the specified key
    /// </summary>
    public class SignOperation : HeaderOperation
    {
        public SignOperation(TezosRpc rpc,
            RequiredValues requiredValues,
            Func<RequiredValues, Task<(ShellHeaderContent, BlockHeaderContent, Signature)>> function) : base(rpc, requiredValues, function)
        {
        }
        
        public InjectOperation InjectBlock => new InjectOperation(Rpc, Values, Apply);
//           tz1TGu6TN5GSez2ndXXeDX6LgUDvLzPLqgYV

        public override async Task<dynamic> ApplyAsync() => await Apply(Values);

        protected override async Task<(ShellHeaderContent, BlockHeaderContent, Signature)> Apply(RequiredValues data)
        {
            var (shell, header, _) = await Function(data);

            var chainId = await Rpc.GetAsync<string>("chains/main/chain_id");
            var watermark = new byte[] {1}.Concat(Base58.Parse(chainId, 3));

            var signature = Key.FromBase58(data.Key).Sign(LocalForge.Concat(
                watermark, 
                LocalForge.ForgeShellHeader(shell), 
                LocalForge.ForgeProtocolData(header.ProtocolData))
            );

            return (shell, header, signature);

        }
        
    }
}