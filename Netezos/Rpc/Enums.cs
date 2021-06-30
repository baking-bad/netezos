using System.Text.Json.Serialization;
using Netezos.Utils.Json;

namespace Netezos.Rpc
{
    /// <summary>
    /// Type of chain
    /// </summary>
    public enum Chain
    {
        Main,
        Test
    }

    /// <summary>
    /// Status of delegate
    /// </summary>
    public enum DelegateStatus
    {
        Active,
        Inactive
    }

    [JsonConverter(typeof(BigMapNormalizationConverter))]
    public enum BigMapNormalization
    {
        Readable,
        Optimized,
        OptimizedLegacy
    }
}
