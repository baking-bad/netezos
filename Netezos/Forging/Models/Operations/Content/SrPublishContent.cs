using System.Text.Json.Serialization;
using Netezos.Encoding;

namespace Netezos.Forging.Models;

public class SrPublishContent : ManagerOperationContent
{
    [JsonPropertyName("kind")]
    public override string Kind => "smart_rollup_publish";

    [JsonPropertyName("rollup")]
    public string Rollup { get; set; }
    
    [JsonPropertyName("commitment")]
    public Commitment Commitment { get; set; }
    
    [JsonPropertyName("origination_proof")]
    public string OriginationProof { get; set; }
    
    [JsonPropertyName("parameters_ty")]
    public MichelinePrim ParametersTy { get; set; }
}

public class Commitment
{
    [JsonPropertyName("compressed_state")]
    public string CompressedState { get; set; }
    
    [JsonPropertyName("inbox_level")]
    public int InboxLevel { get; set; }
    
    [JsonPropertyName("predecessor")]
    public string Predecessor { get; set; }
    
    [JsonPropertyName("number_of_ticks")]
    public long NumberOfTicks { get; set; }
}