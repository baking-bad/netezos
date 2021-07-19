using System.Collections.Generic;
using System.Text.Json.Serialization;
using Netezos.Utils.Json;

namespace Netezos.Forging.Models
{
    public class BlockHeaderContent
    {
        [JsonPropertyName("protocol_data")]
        public ActivationProtocolDataContent ProtocolData { get; set; }
        [JsonPropertyName("operations")]
        public List<HeaderOperationContent> Operations { get; set; }
    }

    [JsonConverter(typeof(HeaderOperationConverter))]
    public class HeaderOperationContent : Operation
    {
        [JsonPropertyName("protocol")]
        public string Protocol { get; set; }
        [JsonPropertyName("error")]
        public List<ErrorContent> Errors { get; set; }
    }

    public class ErrorContent
    {
        [JsonPropertyName("kind")]
        public string Kind { get; set; }
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}