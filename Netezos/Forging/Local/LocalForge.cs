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
            using (MichelineReader reader = new MichelineReader(bytes))
            {
                string branch = reader.ReadBase58(Lengths.B.Decoded, Prefix.B);
                OperationContent content = UnforgeOperation(reader);

                return Task.FromResult((branch, content));
            }
        }

        public Task<(string, IEnumerable<ManagerOperationContent>)> UnforgeOperationGroupAsync(byte[] bytes)
        {
            using (MichelineReader reader = new MichelineReader(bytes))
            {
                string branch = reader.ReadBase58(Lengths.B.Decoded, Prefix.B);
                IEnumerable<ManagerOperationContent> content = UnforgeManagerOperations(reader).ToList();

                return Task.FromResult((branch, content));
            }
        }

        static IEnumerable<ManagerOperationContent> UnforgeManagerOperations(MichelineReader reader)
        {
            while (!reader.EndOfStream)
            {
                yield return (ManagerOperationContent)UnforgeOperation(reader);
            }
        }
    }
}
