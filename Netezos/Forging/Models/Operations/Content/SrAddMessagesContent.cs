using System.Text.Json.Serialization;

namespace Netezos.Forging.Models;

public class SrAddMessagesContent : ManagerOperationContent
{
    [JsonPropertyName("kind")]
    public override string Kind => "smart_rollup_add_messages";
    
    [JsonPropertyName("message")]
    public List<string> Message { get; set; }
}