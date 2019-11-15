using Newtonsoft.Json;

namespace Netezos.Forge.Operations
{
    public class BallotContent : OperationContent
    {
        [JsonProperty("kind")]
        public override string Kind => "ballot";
        
        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("period")]
        public int Period { get; set; }

        [JsonProperty("proposal")]
        public string Proposal { get; set; }

        [JsonProperty("ballot")]
        public Ballots Ballot { get; set; }

        public enum Ballots
        {
            Yay = 0,
            Nay = 1,
            Pass = 2
        }
    }
}
