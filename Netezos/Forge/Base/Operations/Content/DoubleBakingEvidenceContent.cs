using System.Text.Json.Serialization;

namespace Netezos.Forge.Operations
{
    public class DoubleBakingEvidenceContent : OperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "double_baking_evidence";

        [JsonPropertyName("bh1")]
        public BlockHeader BlockHeader1 { get; set; }

        [JsonPropertyName("bh2")]
        public BlockHeader BlockHeader2 { get; set; }
    }
}
