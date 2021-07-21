using System.Collections.Generic;
using System.Text.Json.Serialization;
using Netezos.Utils.Json;

namespace Netezos.Forging.Models
{
    public class BlockHeaderContent
    {
        [JsonPropertyName("chain_id")]
        public string ChainId { get; set; }
        [JsonPropertyName("hash")]
        public string Hash { get; set; }
        [JsonPropertyName("protocol_data")]
        public ProtocolDataContent ProtocolData { get; set; }
        [JsonPropertyName("shell_header")]
        public ShellHeaderContent ShellHeader { get; set; }
    }
}