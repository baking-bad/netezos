using System.Collections.Generic;
using Netezos.Forging.Models;
using Netezos.Keys;

namespace Netezos.Forging.Sandbox
{
    internal class ForwardingParameters
    {
        public ForwardingParameters()
        {
            Operations = new List<List<HeaderOperationContent>>();
        }

        public BlockHeaderContent BlockHeader { get; set; }

        public List<List<HeaderOperationContent>> Operations { get; set; }

        public Signature Signature { get; set; }
    }
}