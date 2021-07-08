using System;
using System.Threading.Tasks;
using Netezos.Encoding;
using Netezos.Forging.Models;
using Netezos.Keys;
using Netezos.Rpc;

namespace Netezos.Forging.Sandbox.Base
{
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
//edsigteZmnR2LSFMmCSiX6kE3MrrnYb98PUKtAuGcTWWm1MxeWNWDBqQa1R49WE8oU3t36mstFgsAaBsM2DffA1pQDDvnLwAXeF
//edsigteZmnR2LSFMmCSiX6kE3MrrnYb98PUKtAuGcTWWm1MxeWNWDBqQa1R49WE8oU3t36mstFgsAaBsM2DffA1pQDDvnLwAXeF
            return (shell, header, signature);

        }
        
    }
}