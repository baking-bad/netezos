using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class RevealContent : ManagerOperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "reveal";

        [JsonPropertyName("public_key")]
        public string PublicKey { get; set; }

        [JsonIgnore]
        internal override OperationTag Tag => OperationTag.Reveal;

        [JsonIgnore]
        internal override uint ValidationGroup => 3;
    }
}
