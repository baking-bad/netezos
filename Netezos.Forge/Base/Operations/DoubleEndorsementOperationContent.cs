using Newtonsoft.Json;

namespace Netezos.Forge.Operations
{
    public class DoubleEndorsementOperationContent
    {
        [JsonProperty("branch")]
        public string Branch { get; }

        [JsonProperty("operations")]
        public EndorsementContent Operations { get; }
        
        [JsonProperty("signature")]
        public string Signature { get; }
        
        public DoubleEndorsementOperationContent(string branch, EndorsementContent operations, string signature)
        {
            Branch = branch;
            Operations = operations;
            Signature = signature;
        }
    }


}
