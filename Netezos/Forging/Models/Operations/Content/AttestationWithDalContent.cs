using System.Numerics;
using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class AttestationWithDalContent : ConsensusOperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "attestation_with_dal";

        [JsonPropertyName("slot")]
        public ushort Slot { get; set; }

        [JsonPropertyName("level")]
        public int Level { get; set; }

        [JsonPropertyName("round")]
        public int Round { get; set; }

        [JsonPropertyName("block_payload_hash")]
        public required string PayloadHash { get; set; }

        [JsonPropertyName("dal_attestation")]
        [JsonConverter(typeof(BigIntegerStringConverter))]
        public BigInteger DalAttestation { get; set; }
    }
}
