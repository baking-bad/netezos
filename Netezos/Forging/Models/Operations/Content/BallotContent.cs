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

        [JsonIgnore]
        internal override OperationTag Tag => OperationTag.Ballot;

        [JsonIgnore]
        internal override uint ValidationGroup => 1;
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Ballot
    {
        Yay  = 0,
        Nay  = 1,
        Pass = 2
    }
}
