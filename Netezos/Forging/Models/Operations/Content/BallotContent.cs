using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class BallotContent : OperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "ballot";
        
        [JsonPropertyName("source")]
        public required string Source { get; set; }

        [JsonPropertyName("period")]
        public int Period { get; set; }

        [JsonPropertyName("proposal")]
        public required string Proposal { get; set; }

        [JsonPropertyName("ballot")]
        public Ballot Ballot { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Ballot
    {
        yay  = 0,
        nay  = 1,
        pass = 2
    }
}
