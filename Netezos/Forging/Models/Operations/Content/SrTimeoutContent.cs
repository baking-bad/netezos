using System.Text.Json.Serialization;

namespace Netezos.Forging.Models;

public class SrTmieoutContent : ManagerOperationContent
{
    [JsonPropertyName("kind")]
    public override string Kind => "smart_rollup_timeout";

    [JsonPropertyName("rollup")]
    public string Rollup { get; set; }
    
    [JsonPropertyName("stakers")]
    public Stakers Stakers { get; set; }
}

public class Stakers
{
    [JsonPropertyName("alice")]
    public string Alice { get; set; }
    
    [JsonPropertyName("bob")]
    public string Bob { get; set; }
}