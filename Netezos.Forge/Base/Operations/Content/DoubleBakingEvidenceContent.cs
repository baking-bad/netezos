using Newtonsoft.Json;

namespace Netezos.Forge.Operations
{
    public class DoubleBakingEvidenceContent : OperationContent
    {
        [JsonProperty("kind")]
        public override string Kind => "double_baking_evidence";

        [JsonProperty("bh1")]
        public BlockHeader BlockHeader1 { get; set; }

        [JsonProperty("bh2")]
        public BlockHeader BlockHeader2 { get; set; }
    }
}
