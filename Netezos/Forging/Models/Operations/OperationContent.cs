using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    [JsonConverter(typeof(OperationContentConverter))]
    public abstract class OperationContent
    {
        [JsonPropertyName("kind")]
        public abstract string Kind { get; }
    }
}
