using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Netezos.Encoding;
using Netezos.Forging.Models;

namespace Netezos.Forging
{
    public partial class LocalForge : IForge, IUnforge
    {
        public Task<byte[]> ForgeOperationAsync(string branch, OperationContent content)
        {
            var branchBytes = Base58.Parse(branch, Prefix.B.Length);
            var contentBytes = ForgeOperation(content);

            return Task.FromResult(branchBytes.Concat(contentBytes));
        }

        public Task<byte[]> ForgeOperationGroupAsync(string branch, IEnumerable<ManagerOperationContent> contents)
        {
            var branchBytes = Base58.Parse(branch, Prefix.B.Length);
            var contentBytes = Concat(contents.Select(ForgeOperation).ToArray());

            return Task.FromResult(branchBytes.Concat(contentBytes));
        }

        public Task<(string, OperationContent)> UnforgeOperationAsync(byte[] bytes)
        {
            string branch = Base58.Convert(bytes.GetBytes(0, Lengths.B.Decoded), Prefix.B);
            OperationContent content = UnforgeOperation(bytes.GetBytes(Lengths.B.Decoded));

            return Task.FromResult((branch, content));
        }

        public Task<(string, IEnumerable<ManagerOperationContent>)> UnforgeOperationGroupAsync(byte[] content)
        {
            throw new System.NotImplementedException();
        }
    }
}
