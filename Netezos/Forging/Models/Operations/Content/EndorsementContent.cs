using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class EndorsementContent : OperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "endorsement";

        [JsonPropertyName("level")]
        public int Level { get; set; }

        [JsonIgnore]
        internal override OperationTag Tag => OperationTag.Endorsement;

        [JsonIgnore]
        internal override uint ValidationGroup => 0;
    }
}
