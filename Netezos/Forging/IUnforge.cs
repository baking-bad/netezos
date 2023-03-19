using Netezos.Forging.Models;

namespace Netezos.Forging
{
    public interface IUnforge
    {
        Task<(string, IEnumerable<OperationContent>)> UnforgeOperationAsync(byte[] content);
    }
}
