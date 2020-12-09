using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class EndorsementContent : OperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "endorsement";

        [JsonPropertyName("level")]
        public int Level { get; set; }
    }
}
