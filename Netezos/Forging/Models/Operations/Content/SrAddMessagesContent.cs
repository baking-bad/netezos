using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class SrAddMessagesContent : ManagerOperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "smart_rollup_add_messages";

        [JsonPropertyName("message")]
        [JsonConverter(typeof(HexListConverter))]
        public required List<byte[]> Messages { get; set; }
    }
}