using System.Text.Json.Serialization;
using Netezos.Micheline;

namespace Netezos.Forge.Operations
{
    public class Parameters
    {
        [JsonPropertyName("entrypoint")]
        public string Entrypoint { get; set; }

        [JsonPropertyName("value")]
        public IMicheline Value { get; set; }
    }
}
