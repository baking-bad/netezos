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
        public IMicheline TicketContent { get; set; } = null!;

        [JsonPropertyName("ticket_ty")]
        public IMicheline TicketType { get; set; } = null!;

        [JsonPropertyName("ticket_ticketer")]
        public string TicketTicketer { get; set; } = null!;

        [JsonPropertyName("ticket_amount")]
        [JsonConverter(typeof(BigIntegerStringConverter))]
        public BigInteger TicketAmount { get; set; }

        [JsonPropertyName("destination")]
        public string Destination { get; set; } = null!;

        [JsonPropertyName("entrypoint")]
        public string Entrypoint { get; set; } = null!;
    }
}
