using System.Text.Json.Serialization;

namespace Netezos.Encoding
{
    public class MichelineBytes : IMicheline
    {
        [JsonIgnore]
        public MichelineType Type => MichelineType.Bytes;

        [JsonPropertyName("bytes")]
        public byte[] Value { get; set; }

        public MichelineBytes(byte[] value) => Value = value;
    }
}
