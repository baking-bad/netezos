using System.Text.Json.Serialization;

namespace Netezos.Forge.Operations
{
    public abstract class OperationContent
    {
        [JsonPropertyName("kind")]
        public abstract string Kind { get; }
    }
}
