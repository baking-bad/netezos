using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Netezos.Utils.Json;

namespace Netezos.Forging.Models
{
    public class ShellHeaderContent
    {
        [JsonPropertyName("level")]
        public int Level { get; set; }
        [JsonPropertyName("proto")]
        public int Proto { get; set; }
        [JsonPropertyName("predecessor")]
        public string Predecessor { get; set; }
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
        [JsonPropertyName("validation_pass")]
        public int ValidationPass { get; set; }
        [JsonPropertyName("operations_hash")]
        public string OperationsHash { get; set; }
        [JsonPropertyName("fitness")]
        public List<string> Fitness { get; set; }
        [JsonPropertyName("context")]
        public string Context { get; set; }
    }

    public class ShellHeaderWithOperations
    {
        [JsonPropertyName("shell_header")]
        public ShellHeaderContent ShellHeader { get; set; }
        [JsonPropertyName("operations")]
        public List<Dictionary<string, List<Operation>>> Operations { get; set; }
    }
}