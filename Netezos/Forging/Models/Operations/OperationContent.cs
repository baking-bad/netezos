using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public abstract class OperationContent
    {
        [JsonPropertyName("kind")]
        public abstract string Kind { get; }
    }
}
