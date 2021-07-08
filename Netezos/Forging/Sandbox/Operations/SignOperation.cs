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
        
        
        public override async Task<dynamic> ApplyAsync()
        {
            return null;
        }
        

        public async Task<(ShellHeaderContent, BlockHeaderContent, Signature)> Apply(RequiredValues data)
        {
            var (shell, header, _) = await Function(data);

            var chainId = await Rpc.GetAsync<string>("chains/main/chain_id");
            var watermark = new byte[] {1}.Concat(Base58.Parse(chainId));

            var key = Key.FromBase58(data.Key);
            var signature = key
                .Sign(watermark.Concat(LocalForge.ForgeShellHeader(shell).Concat(LocalForge.ForgeProtocolData(header.ProtocolData))));

            return (shell, header, signature);

        }
        
    }
}