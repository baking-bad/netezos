using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class DelegationContent : ManagerOperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "delegation";

        [JsonPropertyName("delegate")]
        public string Delegate { get; set; }

        [JsonIgnore]
        internal override OperationTag Tag => OperationTag.Delegation;

        [JsonIgnore]
        internal override uint ValidationGroup => 3;
    }
}
