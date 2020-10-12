using System.Text.Json.Serialization;

namespace Netezos.Micheline
{
    public class MichelineString : IMicheline
    {
        public MichelineNode Type => MichelineNode.String;

        [JsonPropertyName("string")]
        public string Value { get; set; }
    }
}
