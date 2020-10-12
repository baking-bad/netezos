using System.Text.Json.Serialization;

namespace Netezos.Encoding
{
    public class MichelineInt : IMicheline
    {
        [JsonIgnore]
        public MichelineType Type => MichelineType.Int;

        [JsonPropertyName("int")]
        public int Value { get; set; }

        public MichelineInt(int value) => Value = value;
    }
}
