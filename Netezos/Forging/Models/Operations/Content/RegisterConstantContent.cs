using System.Text.Json.Serialization;
using Netezos.Encoding;

namespace Netezos.Forging.Models
{
    public class RegisterConstantContent : ManagerOperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "register_global_constant";

        [JsonPropertyName("value")]
        public IMicheline Value { get; set; }
    }
}
