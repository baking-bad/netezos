using System.Collections.Generic;
using Netezos.Forging.Models;
using Netezos.Keys;

namespace Netezos.Sandbox.Models
{
    public class ForwardingParameters
    {
        public ForwardingParameters()
        {
            Operations = new List<List<MempoolOperation>>();
        }

        public BlockHeaderContent BlockHeader { get; set; }

        public List<List<MempoolOperation>> Operations { get; set; }

        public Signature Signature { get; set; }
    }
}