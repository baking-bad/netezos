using Newtonsoft.Json;

namespace Netezos.Forge.Operations
{
    public class SeedNonceRevelationContent : OperationContent
    {
        [JsonProperty("kind")]
        public override string Kind => "seed_nonce_revelation";

        [JsonProperty("level")]
        public int Level { get; set; }

        [JsonProperty("nonce")]
        public string Nonce { get; set; }
    }
}
