using System.Text.Json.Serialization;
using Netezos.Encoding;

namespace Netezos.Forging.Models
{
    public class OriginationContent : ManagerOperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "origination";

        [JsonPropertyName("balance")]
        [JsonConverter(typeof(Int64StringConverter))]
        public long Balance { get; set; }

        [JsonPropertyName("delegate")]
        public string? Delegate { get; set; }

        [JsonPropertyName("script")]
        public Script Script { get; set; } = null!;
    }

    public class Script
    {
        [JsonPropertyName("code")]
        public MichelineArray Code { get; set; } = null!;

        [JsonPropertyName("storage")]
        public IMicheline Storage { get; set; } = null!;
    }
}
