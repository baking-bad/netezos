using System.Text.Json.Serialization;
using Netezos.Encoding;

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
    public List<Dissection> Step { get; set; }
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

public class Dissection
{
    [JsonPropertyName("state")]
    public string State { get; set; }
    
    [JsonPropertyName("tick")]
    [JsonConverter(typeof(Int64StringConverter))]
    public long Tick { get; set; }
}

public class ProofStep
{
    [JsonPropertyName("pvm_step")]
    public string PvmStep { get; set; }
    
    [JsonPropertyName("input_proof")]
    public InputProof InputProof { get; set;}
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
    
    [JsonPropertyName("serialized_proof")]
    public string SerializedProof { get; set; }
}

public class RevealProof : InputProof
{
    [JsonPropertyName("input_proof_kind")]
    public override string InputProofKind => "reveal_proof";
    
    [JsonPropertyName("reveal_proof")]
    public RevealProofData RevealProofData { get; set; }
}

public class RevealProofData
{
    [JsonPropertyName("reveal_proof_kind")]
    public string RevealProofDataKind { get; set; }
}

public class RawDataProof : RevealProofData
{
    [JsonPropertyName("reveal_proof_kind")]
    public string RevealProofDataKind { get; set; } = "raw_data_proof";
    
    [JsonPropertyName("raw_data")]
    public string RawData { get; set; }
}

public class MetadataProof : RevealProofData
{
    [JsonPropertyName("reveal_proof_kind")]
    public string RevealProofDataKind { get; set; } = "metadata_proof";
}

public class DalPageProof : RevealProofData
{
    [JsonPropertyName("reveal_proof_kind")]
    public string RevealProofDataKind { get; set; } = "dal_page_proof";
    
    [JsonPropertyName("dal_proof")]
    public string DalProof { get; set; }
}
