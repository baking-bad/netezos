using System;
using System.Collections.Generic;
using System.Linq;
using Netezos.Forging.Models;
using Netezos.Keys;

namespace Netezos.Sandbox.Models
{
    public class ForwardingParameters
    {
        public ForwardingParameters()
        {
            Operations = new List<List<MempoolOperation>>();
            ForgedOperations = Array.Empty<List<PreapplyHashOperation>>().ToList();

        }

        public BlockHeaderContent BlockHeader { get; set; }

        public List<List<MempoolOperation>> Operations { get; set; }
        public List<List<PreapplyHashOperation>> ForgedOperations { get; set; }

        public Signature Signature { get; set; }
    }
}