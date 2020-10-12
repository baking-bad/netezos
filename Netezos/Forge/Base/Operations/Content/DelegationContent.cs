using System.Text.Json.Serialization;

namespace Netezos.Forge.Operations
{
    public class DelegationContent : ManagerOperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "delegation";

        [JsonPropertyName("delegate")]
        public string Delegate { get; set; }
    }
}
