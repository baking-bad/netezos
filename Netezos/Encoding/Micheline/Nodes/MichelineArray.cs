using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Netezos.Encoding
{
    public class MichelineArray : List<IMicheline>, IMicheline
    {
        [JsonIgnore]
        public MichelineType Type => MichelineType.Array;
    }
}
