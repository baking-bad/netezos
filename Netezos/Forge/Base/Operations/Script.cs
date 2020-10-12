using System.Text.Json.Serialization;
using Netezos.Micheline;

namespace Netezos.Forge.Operations
{
    public class Script
    {
        [JsonPropertyName("code")]
        public MichelineArray Code { get; set; }

        [JsonPropertyName("storage")]
        public IMicheline Storage { get; set; }
    }
}
