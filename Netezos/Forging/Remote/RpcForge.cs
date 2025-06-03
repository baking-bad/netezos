using Netezos.Encoding;
using Netezos.Forging.Models;
using Netezos.Rpc;

namespace Netezos.Forging
{
    public class RpcForge(TezosRpc rpc) : IForge
    {
        public Task<byte[]> ForgeOperationAsync(OperationContent content)
            => ForgeAsync([content]);

        public Task<byte[]> ForgeOperationAsync(string branch, OperationContent content)
            => ForgeAsync(branch, [content]);

        public Task<byte[]> ForgeOperationGroupAsync(IEnumerable<ManagerOperationContent> contents)
            => ForgeAsync([..contents.Cast<object>()]);

        public Task<byte[]> ForgeOperationGroupAsync(string branch, IEnumerable<ManagerOperationContent> contents)
            => ForgeAsync(branch, [..contents.Cast<object>()]);

        async Task<byte[]> ForgeAsync(List<object> contents)
        {
            var branch = await rpc.Blocks.Head.Hash.GetAsync<string>()
                ?? throw new InvalidOperationException("Branch cannot be null");

            return await ForgeAsync(branch, contents);
        }

        async Task<byte[]> ForgeAsync(string branch, List<object> contents)
        {
            var result = await rpc
                .Blocks
                .Head
                .Helpers
                .Forge
                .Operations
                .PostAsync<string>(branch, contents)
                ?? throw new InvalidOperationException("Forged bytes cannot be null");

            return Hex.Parse(result);
        }
    }
}
