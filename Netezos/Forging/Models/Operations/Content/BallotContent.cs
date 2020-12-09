using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class BallotContent : OperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "ballot";
        
        [JsonPropertyName("source")]
        public string Source { get; set; }

        [JsonPropertyName("period")]
        public int Period { get; set; }

        [JsonPropertyName("proposal")]
        public string Proposal { get; set; }

        [JsonPropertyName("ballot")]
        public Ballot Ballot { get; set; }
    }

    public enum Ballot
    {
        Yay  = 0,
        Nay  = 1,
        Pass = 2
    }
}
