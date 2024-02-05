namespace Netezos.Rpc.Queries;

/// <summary>
/// Rpc query to access the smart rollups data
/// </summary>
public class SmartRollupsQuery : RpcQuery
{
    /// <summary>
    /// Returns the currently active staking parameters for the given delegate.
    /// </summary>
    public SmartRollupsAllQuery All => new(this, "all/");
        
    /// <summary>
    /// Rpc query to access a smart rollup
    /// </summary>
    /// <param name="address">Address of the smart rollup</param>
    /// <returns></returns>
    public SmartRollupQuery this[string address] => new(this, $"smart_rollup/{address}/");
        
    internal SmartRollupsQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

}