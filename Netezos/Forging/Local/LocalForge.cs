using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Netezos.Encoding;
using Netezos.Forging.Models;
using Netezos.Utils;

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
            var contentBytes = Bytes.Concat(contents.Select(ForgeOperation).ToArray());

            return Task.FromResult(branchBytes.Concat(contentBytes));
        }

        public Task<(string, IEnumerable<OperationContent>)> UnforgeOperationAsync(byte[] bytes)
        {
            using (var reader = new ForgedReader(bytes))
            {
                var branch = reader.ReadBase58(Lengths.B.Decoded, Prefix.B);
                var content = new List<OperationContent>();

                while (!reader.EndOfStream)
                {
                    content.Add(UnforgeOperation(reader));
                }

                return Task.FromResult((branch, (IEnumerable<OperationContent>)content));
            }
        }
    }
}
