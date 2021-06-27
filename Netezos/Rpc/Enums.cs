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

    public enum BigMapNormalization
    {
        Readable,
        Optimized,
        Optimized_legacy
    }
}
