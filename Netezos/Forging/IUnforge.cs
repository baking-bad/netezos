using Netezos.Forging.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Netezos.Forging
{
    public interface IUnforge
    {
        Task<(string, IEnumerable<OperationContent>)> UnforgeOperationAsync(byte[] content);
    }
}
