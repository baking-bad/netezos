using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    [JsonConverter(typeof(ConsensusOperationContentConverter))]
    public abstract class ConsensusOperationContent : OperationContent { }
}
