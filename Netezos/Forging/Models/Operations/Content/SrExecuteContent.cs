using System.Text.Json.Serialization;

namespace Netezos.Forging.Models;

public class SrExecuteContent : ManagerOperationContent
{
    [JsonPropertyName("kind")]
    public override string Kind => "smart_rollup_execute_outbox_message";

    [JsonPropertyName("rollup")]
    public string Rollup { get; set; }
    
    [JsonPropertyName("cemented_commitment")]
    public string CementedCommitment { get; set; }
    
    [JsonConverter(typeof(HexConverter))]
    [JsonPropertyName("output_proof")]
    public byte[] OutputProof { get; set; }
}