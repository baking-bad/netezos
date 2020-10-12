using System.Text.Json.Serialization;

namespace Netezos.Micheline
{
    public class MichelineBytes : IMicheline
    {
        public MichelineNode Type => MichelineNode.Bytes;

        [JsonPropertyName("bytes")]
        public byte[] Value { get; set; }
    }
}
