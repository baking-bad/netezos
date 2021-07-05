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

    [JsonConverter(typeof(NormalizationConverter))]
    public enum Normalization
    {
        Readable,
        Optimized,
        OptimizedLegacy
    }
}
