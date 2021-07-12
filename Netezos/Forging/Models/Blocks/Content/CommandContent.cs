using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class CommandContent
    {
        [JsonPropertyName("command")]
        public string Command { get; set; }
    }
}