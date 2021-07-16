using System.Text.Json.Serialization;
using Netezos.Encoding;

namespace Netezos.Forging.Models
{
    public class TransactionContent : ManagerOperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "transaction";

        [JsonPropertyName("amount")]
        [JsonConverter(typeof(Int64StringConverter))]
        public long Amount { get; set; }

        [JsonPropertyName("destination")]
        public string Destination { get; set; }

        [JsonPropertyName("parameters")]
        public Parameters Parameters { get; set; }

        [JsonIgnore]
        internal override OperationTag Tag => OperationTag.Transaction;

        [JsonIgnore]
        internal override uint ValidationGroup => 3;
    }

    public class Parameters
    {
        [JsonPropertyName("entrypoint")]
        public string Entrypoint { get; set; }

        [JsonPropertyName("value")]
        public IMicheline Value { get; set; }
    }
}