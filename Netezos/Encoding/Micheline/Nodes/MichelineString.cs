using System.Text.Json.Serialization;

namespace Netezos.Encoding
{
    public class MichelineString : IMicheline
    {
        [JsonIgnore]
        public MichelineType Type => MichelineType.String;

        [JsonPropertyName("string")]
        public string Value { get; set; }

        public MichelineString(string value) => Value = value;
    }
}
