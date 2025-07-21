using System.Numerics;
using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class AttestationsAggregateContent : ConsensusOperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "attestations_aggregate";

        [JsonPropertyName("consensus_content")]
        public required ConsensusContent ConsensusContent { get; set; }

        [JsonPropertyName("committee")]
        public required List<CommitteeMember> Committee { get; set; }
    }

    public class CommitteeMember
    {
        [JsonPropertyName("slot")]
        public ushort Slot { get; set; }

        [JsonPropertyName("dal_attestation")]
        [JsonConverter(typeof(BigIntegerNullableStringConverter))]
        public BigInteger? DalAttestation { get; set; }
    }
}
