using System.Numerics;
using System.Text.Json.Serialization;
using Netezos.Encoding;

namespace Netezos.Forging.Models
{
    public class TransferTicketContent : ManagerOperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "transfer_ticket";

        [JsonPropertyName("ticket_contents")]
        public IMicheline TicketContent { get; set; }

        [JsonPropertyName("ticket_ty")]
        public IMicheline TicketType { get; set; }

        [JsonPropertyName("ticket_ticketer")]
        public string TicketTicketer { get; set; }

        [JsonPropertyName("ticket_amount")]
        [JsonConverter(typeof(BigIntegerStringConverter))]
        public BigInteger TicketAmount { get; set; }

        [JsonPropertyName("destination")]
        public string Destination { get; set; }

        [JsonPropertyName("entrypoint")]
        public string Entrypoint { get; set; }
    }
}
