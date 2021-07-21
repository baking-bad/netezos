using System.Collections.Generic;
using System.Text.Json.Serialization;
using Netezos.Utils.Json;

namespace Netezos.Forging.Models
{
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