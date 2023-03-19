using Netezos.Forging.Models;

namespace Netezos.Forging
{
    public interface IForge
    {
        Task<byte[]> ForgeOperationAsync(string branch, OperationContent content);

        Task<byte[]> ForgeOperationGroupAsync(string branch, IEnumerable<ManagerOperationContent> contents);
    }
}
