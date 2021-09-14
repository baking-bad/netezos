using System;
using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class BakingRightsContent
    {
        [JsonPropertyName("level")]
        public int Level { get; set; }
        [JsonPropertyName("delegate")]
        public string Delegate { get; set; }
        [JsonPropertyName("priority")]
        public int Priority { get; set; }
        [JsonPropertyName("estimated_time")]
        public DateTime EstimateTime { get; set; }
    }
}