using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Netezos.Micheline
{
    public class MichelinePrim : IMicheline
    {
        public MichelineNode Type => MichelineNode.Prim;

        [JsonPropertyName("prim")]
        public PrimType Prim { get; set; }

        [JsonPropertyName("args")]
        public List<IMicheline> Args { get; set; }

        [JsonPropertyName("annots")]
        public List<string> Annots { get; set; }
    }
}
