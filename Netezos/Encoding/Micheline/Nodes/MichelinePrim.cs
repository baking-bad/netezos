using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Netezos.Encoding
{
    public class MichelinePrim : IMicheline
    {
        [JsonIgnore]
        public MichelineType Type => MichelineType.Prim;

        [JsonPropertyName("prim")]
        public PrimType Prim { get; set; }

        [JsonPropertyName("args")]
        public List<IMicheline> Args { get; set; }

        [JsonPropertyName("annots")]
        public List<IAnnotation> Annots { get; set; }
    }
}
