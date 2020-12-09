using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Netezos.Encoding;
using Netezos.Forging.Models;

namespace Netezos.Forging
{
    public partial class LocalForge : IForge
    {
        public Task<byte[]> ForgeOperationAsync(string branch, OperationContent content)
        {
            var branchBytes = Base58.Parse(branch, 2);
            var contentBytes = ForgeOperation(content);

            return Task.FromResult(branchBytes.Concat(contentBytes));
        }

        public Task<byte[]> ForgeOperationGroupAsync(string branch, IEnumerable<ManagerOperationContent> contents)
        {
            var branchBytes = Base58.Parse(branch, 2);
            var contentBytes = Concat(contents.Select(ForgeOperation).ToArray());

            return Task.FromResult(branchBytes.Concat(contentBytes));
        }
    }
}