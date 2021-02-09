using Netezos.Forging.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Netezos.Forging
{
    public interface IUnforge
    {
        Task<(string, OperationContent)> UnforgeOperationAsync(byte[] content);

        Task<(string, IEnumerable<ManagerOperationContent>)> UnforgeOperationGroupAsync(byte[] content);
    }
}