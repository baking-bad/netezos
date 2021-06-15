using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class EndorsementWithSlotContent : OperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "endorsement_with_slot";

        [JsonPropertyName("endorsement")]
        public EndorsementWithSlotWrapper Endorsement { get; set; }

        [JsonPropertyName("slot")]
        public int Slot { get; set; }
    }

    public class EndorsementWithSlotWrapper
    {
        [JsonPropertyName("branch")]
        public string Branch { get; set; }

        [JsonPropertyName("operations")]
        public EndorsementContent Operations { get; set; }

        [JsonPropertyName("signature")]
        public string Signature { get; set; }
    }
}
