using System.Text.Json.Serialization;

namespace Netezos.Forging.Models;

public class SrCementContent : ManagerOperationContent
{
    [JsonPropertyName("kind")]
    public override string Kind => "smart_rollup_cement";

    //Base58
    [JsonPropertyName("rollup")]
    public string Rollup { get; set; }
    
    //Base58
    [JsonPropertyName("commitment")]
    public string Commitment { get; set; }
}