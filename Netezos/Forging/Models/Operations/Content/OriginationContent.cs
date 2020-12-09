using System.Text.Json.Serialization;
using Netezos.Encoding;

namespace Netezos.Forging.Models
{
    public class OriginationContent : ManagerOperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "origination";

        [JsonPropertyName("balance")]
        public long Balance { get; set; }

        [JsonPropertyName("delegate")]
        public string Delegate { get; set; }

        [JsonPropertyName("script")]
        public Script Script { get; set; }
    }

    public class Script
    {
        [JsonPropertyName("code")]
        public MichelineArray Code { get; set; }

        [JsonPropertyName("storage")]
        public IMicheline Storage { get; set; }
    }
}
