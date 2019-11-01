using System.Collections.Generic;
using System.Threading.Tasks;

using Netezos.Forge.Operations;

namespace Netezos.Forge
{
    public interface IForge
    {
        Task<byte[]> ForgeOperationAsync(string branch, OperationContent content);

        Task<byte[]> ForgeOperationGroupAsync(string branch, IEnumerable<ManagerOperationContent> contents);
    }
}
