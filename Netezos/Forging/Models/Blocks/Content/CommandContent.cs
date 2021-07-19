using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public abstract class CommandContent
    {
        [JsonPropertyName("command")]
        public abstract string Command { get; }
    }
}