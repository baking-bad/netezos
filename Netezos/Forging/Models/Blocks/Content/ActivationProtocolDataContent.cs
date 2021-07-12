using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class ActivationProtocolDataContent : ProtocolDataContent
    {
        [JsonPropertyName("content")]
        public ActivationCommandContent Content { get; set; }
    }
    
    public class ActivationCommandContent : CommandContent
    {
        [JsonPropertyName("command")]
        public string Command => "activate";
        [JsonPropertyName("hash")]
        public string Hash { get; set; }
        [JsonPropertyName("fitness")]
        public List<string> Fitness { get; set; }
        [JsonPropertyName("protocol_parameters")]
        public string ProtocolParameters { get; set; }
    }
}