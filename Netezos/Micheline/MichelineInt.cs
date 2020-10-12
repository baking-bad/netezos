using System.Text.Json.Serialization;

namespace Netezos.Micheline
{
    public class MichelineInt : IMicheline
    {
        public MichelineNode Type => MichelineNode.Int;

        [JsonPropertyName("int")]
        public int Value { get; set; }
    }
}
