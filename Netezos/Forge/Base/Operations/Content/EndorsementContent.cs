using System.Text.Json.Serialization;

namespace Netezos.Forge.Operations
{
    public class EndorsementContent : OperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "endorsement";

        [JsonPropertyName("level")]
        public int Level { get; set; }
    }
}
