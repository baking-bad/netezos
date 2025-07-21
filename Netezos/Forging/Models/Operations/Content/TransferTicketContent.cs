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
        public required IMicheline TicketContent { get; set; }

        [JsonPropertyName("ticket_ty")]
        public required IMicheline TicketType { get; set; }

        [JsonPropertyName("ticket_ticketer")]
        public required string TicketTicketer { get; set; }

        [JsonPropertyName("ticket_amount")]
        [JsonConverter(typeof(BigIntegerStringConverter))]
        public BigInteger TicketAmount { get; set; }

        [JsonPropertyName("destination")]
        public required string Destination { get; set; }

        [JsonPropertyName("entrypoint")]
        public required string Entrypoint { get; set; }
    }
}
