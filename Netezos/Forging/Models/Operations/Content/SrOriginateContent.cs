using System.Text.Json.Serialization;
using Netezos.Encoding;

namespace Netezos.Forging.Models;

public class SrOriginateContent : ManagerOperationContent
{
    [JsonPropertyName("kind")]
    public override string Kind => "smart_rollup_originate";

    [JsonPropertyName("pvm_kind")]
    public string PvmKind { get; set; }
    
    [JsonPropertyName("kernel")]
    public string Kernel { get; set; }
    
    [JsonPropertyName("origination_proof")]
    public string OriginationProof { get; set; }
    
    [JsonPropertyName("parameters_ty")]
    public IMicheline ParametersTy { get; set; }
}