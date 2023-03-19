using System.Text.Json;
using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class TxRollupRejectionContent : ManagerOperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "tx_rollup_rejection";

        [JsonPropertyName("rollup")]
        public string Rollup { get; set; } = null!;

        [JsonPropertyName("level")]
        public int Level { get; set; }

        [JsonPropertyName("message")]
        public TxRollupRejectionMessage Message { get; set; } = null!;

        [JsonPropertyName("message_position")]
        [JsonConverter(typeof(Int64StringConverter))]
        public long MessagePosition { get; set; }

        [JsonPropertyName("message_path")]
        public List<string> MessagePath { get; set; } = null!; // var

        [JsonPropertyName("message_result_hash")]
        public string MessageResultHash { get; set; } = null!; // 32

        [JsonPropertyName("message_result_path")]
        public List<string> MessageResultPath { get; set; } = null!; // var

        [JsonPropertyName("previous_message_result")]
        public TxRollupMessageResult PreviousMessageResult { get; set; } = null!;

        [JsonPropertyName("previous_message_result_path")]
        public List<string> PreviousMessageResultPath { get; set; } = null!; // var

        [JsonPropertyName("proof")]
        public JsonElement Proof { get; set; }
    }

    public class TxRollupRejectionMessage
    {
        [JsonPropertyName("batch")]
        public string Batch { get; set; } = null!; // var

        [JsonPropertyName("deposit")]
        public TxRollupRejectionDepositMessage Deposit { get; set; } = null!;
    }

    public class TxRollupRejectionDepositMessage
    {
        [JsonPropertyName("sender")]
        public string Sender { get; set; } = null!; // 21

        [JsonPropertyName("destination")]
        public string Destination { get; set; } = null!; // 20

        [JsonPropertyName("ticket_hash")]
        public string TicketHash { get; set; } = null!; // 32

        [JsonPropertyName("amount")]
        [JsonConverter(typeof(Int64StringConverter))]
        public long Amount { get; set; }
    }

    public class TxRollupMessageResult
    {
        [JsonPropertyName("context_hash")]
        public string ContextHash { get; set; } = null!; // 32

        [JsonPropertyName("withdraw_list_hash")]
        public string WithdrawListHash { get; set; } = null!; // 32
    }
}
