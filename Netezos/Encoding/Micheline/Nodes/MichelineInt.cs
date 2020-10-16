using System.Numerics;
using System.Text.Json.Serialization;

namespace Netezos.Encoding
{
    public class MichelineInt : IMicheline
    {
        [JsonIgnore]
        public MichelineType Type => MichelineType.Int;

        [JsonPropertyName("int")]
        public BigInteger Value { get; set; }

        public MichelineInt(BigInteger value) => Value = value;
    }
}
