using System.Text.Json.Serialization;
using Netezos.Encoding;

namespace Netezos.Forging.Models
{
    public class SrOriginateContent : ManagerOperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "smart_rollup_originate";

        [JsonPropertyName("pvm_kind")]
        public PvmKind PvmKind { get; set; }

        [JsonConverter(typeof(HexConverter))]
        [JsonPropertyName("kernel")]
        public byte[] Kernel { get; set; } = null!;

        [JsonPropertyName("origination_proof")]
        [JsonConverter(typeof(HexConverter))]
        public byte[] OriginationProof { get; set; } = null!;

        [JsonPropertyName("parameters_ty")]
        public IMicheline ParametersType { get; set; } = null!;
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PvmKind
    {
        Arith = 0,
        Wasm_2_0_0 = 1
    }
}