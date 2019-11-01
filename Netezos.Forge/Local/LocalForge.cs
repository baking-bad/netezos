using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Netezos.Forge.Operations;
using Netezos.Forge.Utils;

namespace Netezos.Forge
{
    public class LocalForge : IForge
    {
        public Task<byte[]> ForgeOperationAsync(string branch, OperationContent content)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> ForgeOperationGroupAsync(string branch, IEnumerable<ManagerOperationContent> contents)
        {
            throw new NotImplementedException();
        }
    }
}
