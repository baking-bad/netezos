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

public class Refutation
{
    [JsonPropertyName("refutation_kind")]
    public string RefutationKind { get; set; }
    
    [JsonPropertyName("player_commitment_hash")]
    public string PlayerCommitmentHash { get; set; }
    
    [JsonPropertyName("opponent_commitment_hash")]
    public string OpponentCommitmentHash { get; set; }
}