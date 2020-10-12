using System.Text.Json.Serialization;

namespace Netezos.Forge.Operations
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
}
