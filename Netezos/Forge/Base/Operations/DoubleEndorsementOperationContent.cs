using System.Text.Json.Serialization;

namespace Netezos.Forge.Operations
{
    public class DoubleEndorsementOperationContent
    {
        [JsonPropertyName("branch")]
        public string Branch { get; }

        [JsonPropertyName("operations")]
        public EndorsementContent Operations { get; }
        
        [JsonPropertyName("signature")]
        public string Signature { get; }
        
        public DoubleEndorsementOperationContent(string branch, EndorsementContent operations, string signature)
        {
            Branch = branch;
            Operations = operations;
            Signature = signature;
        }
    }


}
