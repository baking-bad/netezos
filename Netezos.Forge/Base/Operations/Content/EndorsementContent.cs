using Newtonsoft.Json;

namespace Netezos.Forge.Operations
{
    public class EndorsementContent : OperationContent
    {
        [JsonProperty("kind")]
        public override string Kind => "endorsement";

        [JsonProperty("level")]
        public int Level { get; set; }
    }
}
