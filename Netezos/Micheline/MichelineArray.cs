using System.Collections.Generic;

namespace Netezos.Micheline
{
    public class MichelineArray : List<IMicheline>, IMicheline
    {
        public MichelineNode Type => MichelineNode.Array;
    }
}
