using Netezos.Forging.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Netezos.Forging
{
    public interface IUnforge
    {
        Task<OperationContent> UnforgeOperationAsync(byte[] content);

        Task<IEnumerable<ManagerOperationContent>> UnforgeOperationGroupAsync(byte[] content);
    }
}