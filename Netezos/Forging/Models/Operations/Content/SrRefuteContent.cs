using System.Text.Json.Serialization;

namespace Netezos.Forging.Models;

public class SrRefuteContent : ManagerOperationContent
{
    [JsonPropertyName("kind")]
    public override string Kind => "smart_rollup_refute";

    [JsonPropertyName("rollup")]
    public string Rollup { get; set; }
    
    [JsonPropertyName("opponent")]
    public string Opponent { get; set; }
    
    [JsonPropertyName("refutation")]
    public Refutation Refutation { get; set; }
}

[JsonConverter(typeof(RefutationConverter))]
public abstract class Refutation
{
    [JsonPropertyName("refutation_kind")]
    public abstract string RefutationKind { get; }
}

public class RefutationStart : Refutation
{
    [JsonPropertyName("refutation_kind")] 
    public override string RefutationKind => "start";
    
    [JsonPropertyName("player_commitment_hash")]
    public string PlayerCommitmentHash { get; set; }
    
    [JsonPropertyName("opponent_commitment_hash")]
    public string OpponentCommitmentHash { get; set; }
}

public class RefutationDissectionMove : Refutation
{
    [JsonPropertyName("refutation_kind")] 
    public override string RefutationKind => "move";
    
    [JsonPropertyName("choice")]
    [JsonConverter(typeof(Int64StringConverter))]
    public long Choice { get; set; }
    
    [JsonPropertyName("step")]
    public List<DissectionStep> Step { get; set; }
}

public class RefutationProofMove : Refutation
{
    [JsonPropertyName("refutation_kind")] 
    public override string RefutationKind => "move";
    
    [JsonPropertyName("choice")]
    [JsonConverter(typeof(Int64StringConverter))]
    public long Choice { get; set; }
    
    [JsonPropertyName("step")]
    public ProofStep Step { get; set; }
}

public class DissectionStep
{
    [JsonPropertyName("state")]
    public string? State { get; set; }
    
    [JsonPropertyName("tick")]
    [JsonConverter(typeof(Int64StringConverter))]
    public long Tick { get; set; }
}

public class ProofStep
{
    [JsonConverter(typeof(HexConverter))]
    [JsonPropertyName("pvm_step")]
    public byte[] PvmStep { get; set; }
    
    [JsonPropertyName("input_proof")]
    public InputProof? InputProof { get; set;}
}

[JsonConverter(typeof(InputProofConverter))]
public abstract class InputProof
{
    [JsonPropertyName("input_proof_kind")]
    public abstract string InputProofKind { get; }
}

public class FirstInput : InputProof
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
    
    [JsonConverter(typeof(HexConverter))]
    [JsonPropertyName("serialized_proof")]
    public byte[] SerializedProof { get; set; }
}

public class RevealProof : InputProof
{
    [JsonPropertyName("input_proof_kind")]
    public override string InputProofKind => "reveal_proof";
    
    [JsonPropertyName("reveal_proof")]
    public RevealProofData RevealProofData { get; set; }
}

public abstract class RevealProofData
{
    [JsonPropertyName("reveal_proof_kind")]
    public abstract string RevealProofDataKind { get; }
}

public class RawDataProof : RevealProofData
{
    [JsonPropertyName("reveal_proof_kind")]
    public override string RevealProofDataKind => "raw_data_proof";
    
    [JsonConverter(typeof(HexConverter))]
    [JsonPropertyName("raw_data")]
    public byte[] RawData { get; set; }
}

public class MetadataProof : RevealProofData
{
    [JsonPropertyName("reveal_proof_kind")]
    public override string RevealProofDataKind => "metadata_proof";
}

public class DalPageProof : RevealProofData
{
    [JsonPropertyName("reveal_proof_kind")]
    public override string RevealProofDataKind => "dal_page_proof";
    
    [JsonConverter(typeof(HexConverter))]
    [JsonPropertyName("dal_proof")]
    public byte[] DalProof { get; set; }
}
