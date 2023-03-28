using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class SrRefuteContent : ManagerOperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "smart_rollup_refute";

        [JsonPropertyName("rollup")]
        public string Rollup { get; set; } = null!;

        [JsonPropertyName("opponent")]
        public string Opponent { get; set; } = null!;

        [JsonPropertyName("refutation")]
        public RefutationMove Refutation { get; set; } = null!;
    }

    [JsonConverter(typeof(RefutationMoveConverter))]
    public abstract class RefutationMove
    {
        [JsonPropertyName("refutation_kind")]
        public abstract string RefutationKind { get; }
    }

    public class RefutationStart : RefutationMove
    {
        [JsonPropertyName("refutation_kind")]
        public override string RefutationKind => "start";

        [JsonPropertyName("player_commitment_hash")]
        public string PlayerCommitment { get; set; } = null!;

        [JsonPropertyName("opponent_commitment_hash")]
        public string OpponentCommitment { get; set; } = null!;
    }

    public class RefutationDissection : RefutationMove
    {
        [JsonPropertyName("refutation_kind")]
        public override string RefutationKind => "move";

        [JsonPropertyName("choice")]
        [JsonConverter(typeof(Int64StringConverter))]
        public long Choice { get; set; }

        [JsonPropertyName("step")]
        public List<DissectionStep> Steps { get; set; } = null!;
    }

    public class DissectionStep
    {
        [JsonPropertyName("state")]
        public string? State { get; set; }

        [JsonPropertyName("tick")]
        [JsonConverter(typeof(Int64StringConverter))]
        public long Tick { get; set; }
    }

    public class RefutationProof : RefutationMove
    {
        [JsonPropertyName("refutation_kind")]
        public override string RefutationKind => "move";

        [JsonPropertyName("choice")]
        [JsonConverter(typeof(Int64StringConverter))]
        public long Choice { get; set; }

        [JsonPropertyName("step")]
        public ProofStep Step { get; set; } = null!;
    }

    public class ProofStep
    {
        [JsonPropertyName("pvm_step")]
        [JsonConverter(typeof(HexConverter))]
        public byte[] PvmStep { get; set; } = null!;

        [JsonPropertyName("input_proof")]
        public InputProof? InputProof { get; set; }
    }

    [JsonConverter(typeof(InputProofConverter))]
    public abstract class InputProof
    {
        [JsonPropertyName("input_proof_kind")]
        public abstract string InputProofKind { get; }
    }

    public class FirstInputProof : InputProof
    {
        [JsonPropertyName("input_proof_kind")]
        public override string InputProofKind => "first_input";
    }

    public class InboxProof : InputProof
    {
        [JsonPropertyName("input_proof_kind")]
        public override string InputProofKind => "inbox_proof";

        [JsonPropertyName("level")]
        public int Level { get; set; }

        [JsonPropertyName("message_counter")]
        [JsonConverter(typeof(Int64StringConverter))]
        public long MessageCounter { get; set; }

        [JsonPropertyName("serialized_proof")]
        [JsonConverter(typeof(HexConverter))]
        public byte[] Proof { get; set; } = null!;
    }

    public class RevealProof : InputProof
    {
        [JsonPropertyName("input_proof_kind")]
        public override string InputProofKind => "reveal_proof";

        [JsonPropertyName("reveal_proof")]
        public RevealProofData Proof { get; set; } = null!;
    }

    [JsonConverter(typeof(RevealProofDataConverter))]
    public abstract class RevealProofData
    {
        [JsonPropertyName("reveal_proof_kind")]
        public abstract string RevealProofDataKind { get; }
    }

    public class MetadataProof : RevealProofData
    {
        [JsonPropertyName("reveal_proof_kind")]
        public override string RevealProofDataKind => "metadata_proof";
    }

    public class RawDataProof : RevealProofData
    {
        [JsonPropertyName("reveal_proof_kind")]
        public override string RevealProofDataKind => "raw_data_proof";

        [JsonPropertyName("raw_data")]
        [JsonConverter(typeof(HexConverter))]
        public byte[] RawData { get; set; } = null!;
    }

    public class DalPageProof : RevealProofData
    {
        [JsonPropertyName("reveal_proof_kind")]
        public override string RevealProofDataKind => "dal_page_proof";

        [JsonPropertyName("dal_page_id")]
        public DalPageId DalPageId { get; set; } = null!;

        [JsonPropertyName("dal_proof")]
        [JsonConverter(typeof(HexConverter))]
        public byte[] Proof { get; set; } = null!;
    }

    public class DalPageId
    {
        [JsonPropertyName("published_level")]
        public int PublishedLevel { get; set; }

        [JsonPropertyName("slot_index")]
        public int SlotIndex { get; set; }

        [JsonPropertyName("page_index")]
        public int PageIndex { get; set; }
    }
}