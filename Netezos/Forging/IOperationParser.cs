using Netezos.Forging.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Netezos.Forging
{
    public interface IOperationParser
    {
        Task<OperationContent> ParseOperationAsync(byte[] content);

        Task<IEnumerable<ManagerOperationContent>> ParseOperationGroupAsync(byte[] content);
    }
}