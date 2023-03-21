using System.Text.Json.Serialization;

namespace Netezos.Forging.Models;

public class SrExecuteContent : ManagerOperationContent
{
    [JsonPropertyName("kind")]
    public override string Kind => "smart_rollup_execute_outbox_message";

    //Base58
    [JsonPropertyName("rollup")]
    public string Rollup { get; set; }
    
    //Base58
    [JsonPropertyName("cemented_commitment")]
    public string CementedCommitment { get; set; }
    
    //Base58
    [JsonPropertyName("output_proof")]
    public string OutputProof { get; set; }
}