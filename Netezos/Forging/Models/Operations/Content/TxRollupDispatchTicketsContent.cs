using System.Text.Json.Serialization;
using Netezos.Encoding;

namespace Netezos.Forging.Models
{
    public class TxRollupDispatchTicketsContent : ManagerOperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "tx_rollup_dispatch_tickets";

        [JsonPropertyName("tx_rollup")]
        public string Rollup { get; set; } = null!;

        [JsonPropertyName("level")]
        public int Level { get; set; }

        [JsonPropertyName("context_hash")]
        public string ContextHash { get; set; } = null!;

        [JsonPropertyName("message_index")]
        public int MessageIndex { get; set; }

        [JsonPropertyName("message_result_path")]
        public List<string> MessageResultPath { get; set; } = null!;

        public List<TxRollupTicketsInfo> TicketsInfo { get; set; } = null!;
    }

    public class TxRollupTicketsInfo
    {
        [JsonPropertyName("contents")]
        public IMicheline Contents { get; set; } = null!;

        [JsonPropertyName("ty")]
        public IMicheline Type { get; set; } = null!;

        [JsonPropertyName("ticketer")]
        public string Ticketer { get; set; } = null!;

        [JsonPropertyName("amount")]
        [JsonConverter(typeof(Int64StringConverter))]
        public long Amount { get; set; }

        [JsonPropertyName("claimer")]
        public string Claimer { get; set; } = null!;
    }
}
